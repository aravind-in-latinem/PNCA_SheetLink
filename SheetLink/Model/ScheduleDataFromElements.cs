using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

namespace PNCA_SheetLink.SheetLink.Model
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ScheduleDataFromElements

    {
        public ViewSchedule ScheduleView { get; set; }
        public List<ScheduledElement> ScheduledElements { get; set; } = new List<ScheduledElement>();

        List<string> fromRoomParamNames = new List<string>();
        List<string> toRoomParamNames = new List<string>();

        public ScheduleDataFromElements(ViewSchedule scheduleView)
        {
            ScheduleView = scheduleView;
        }
        public DataTable CreateScheduleDataTable(Document document)
        {
            var dataTableBuilder = new DataTableCreator();
            #region ViewCollector
            var visibleElem = new FilteredElementCollector(document, ScheduleView.Id).ToElements();
            var scheduleFieldCount = ScheduleView.Definition.GetFieldCount();
            var paramIdFIeldIndexPair = new Dictionary<ElementId, int>();


            #endregion
            for (int i = 0; i < scheduleFieldCount; i++)
            {
                var fieldParamElemId = ScheduleView.Definition.GetField(i).ParameterId;
                var fieldIndex = ScheduleView.Definition.GetField(i).FieldIndex;
                if (fieldParamElemId != new ElementId(Convert.ToInt64(-1)))
                {
                    if (!paramIdFIeldIndexPair.ContainsKey(fieldParamElemId))
                        paramIdFIeldIndexPair.Add(fieldParamElemId, fieldIndex);
                }
                else if (ScheduleView.Definition.GetField(i).IsCombinedParameterField)
                {
                    var combinedParameters = ScheduleView.Definition.GetField(i).GetCombinedParameters().ToList().Select(a => a.ParamId);
                    foreach (var paramId in combinedParameters)
                    {
                        if (!paramIdFIeldIndexPair.ContainsKey(paramId))
                            paramIdFIeldIndexPair.Add(paramId, fieldIndex);
                    }
                }
                if (ScheduleView.Definition.GetField(i).FieldType == ScheduleFieldType.FromRoom)
                {
                    fromRoomParamNames.Add(ScheduleView.Definition.GetField(i).GetName());
                }
                if (ScheduleView.Definition.GetField(i).FieldType == ScheduleFieldType.ToRoom)
                {
                    toRoomParamNames.Add(ScheduleView.Definition.GetField(i).GetName());
                }
            }
            //iterating through visible elements and sending respective parameters to parameter processors.
            foreach (var elem in visibleElem)
            {

                var scheduledElement = new ScheduledElement();
                scheduledElement.RowElementId = elem.Id;
                //handle instance parameters
                var instanceParameterSet = elem.Parameters.OfType<Parameter>().ToList();
                scheduledElement.ScheduledFields.AddRange(processParameters(elem, paramIdFIeldIndexPair, "Instance", instanceParameterSet));

                //handle type parameters
                var elemSymbol = document.GetElement(elem.GetTypeId());
                var typeParameterSet = elemSymbol.Parameters.OfType<Parameter>().ToList();
                scheduledElement.ScheduledFields.AddRange(processParameters(elem, paramIdFIeldIndexPair, "Type", typeParameterSet));

                //handle room parameters
                var fromRoomParameterSet = new List<Parameter>();
                var toRoomParameterSet = new List<Parameter>();
                if (fromRoomParamNames.Count != 0)
                {
                    fromRoomParameterSet = (elem as FamilyInstance).FromRoom?.Parameters.OfType<Parameter>().ToList() ?? new List<Parameter>();
                }
                if (toRoomParamNames.Count != 0)
                {
                    toRoomParameterSet = (elem as FamilyInstance).ToRoom?.Parameters.OfType<Parameter>().ToList() ?? new List<Parameter>();
                }
                var roomParameterSet = fromRoomParameterSet.Concat(toRoomParameterSet).ToList();
                scheduledElement.ScheduledFields.AddRange(processParameters(elem, paramIdFIeldIndexPair, "Room", roomParameterSet));

                ScheduledElements.Add(scheduledElement);

            }
            var dataTable = dataTableBuilder.PrepareTableData(ScheduledElements);

            //TaskDialog.Show("Success", "Read Success");

            return dataTable;
        }


        public List<ScheduledField> processParameters(Element elem, Dictionary<ElementId, int> fieldIds, string parameterType, List<Parameter> parameterCollection)
        {
            List<ScheduledField> scheduledFields = new List<ScheduledField>();

            foreach (var p in parameterCollection)
            {
                if (fieldIds.Keys.Contains(p.Id))
                {
                    ScheduledField field = new ScheduledField();
                    field.ParameterElement = p;
                    if (!parameterType.Equals("Room"))
                        field.FieldName = p.Definition.Name;
                    if (parameterType.Equals("Room"))
                    {
                        if (fromRoomParamNames.Contains(String.Concat("From Room: ", p.Definition.Name)))
                            field.FieldName = "From Room: " + p.Definition.Name;
                        else if (toRoomParamNames.Contains(String.Concat("To Room: ", p.Definition.Name)))
                            field.FieldName = "To Room: " + p.Definition.Name;
                    }
                    if (!String.IsNullOrEmpty(field.FieldName))
                    {
                        field.FieldValue = p.AsString() ?? p.AsValueString() ?? string.Empty;
                        field.SelectedElementId = elem.Id;
                        field.ParameterType = parameterType;
                        field.FieldIndex = fieldIds[p.Id];
                        scheduledFields.Add(field);
                    }
                }
            }

            return scheduledFields;
        }



    }
}
