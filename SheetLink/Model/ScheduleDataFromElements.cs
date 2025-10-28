using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

namespace PNCA_SheetLink.SheetLink.Model
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ScheduleDataFromElements

    {
        public  ViewSchedule ScheduleView { get; set; }
        public List<ScheduledElement> ScheduledElements { get; set; } = new List<ScheduledElement>();

        public ScheduleDataFromElements(ViewSchedule scheduleView)
        {
            ScheduleView = scheduleView;
        }
        public DataTable CreateScheduleDataTable(Document document)
        {
            var dataTableBuilder = new ScheduleDataBuilder();
            #region ViewCollector
            var visibleElem = new FilteredElementCollector(document, ScheduleView.Id).ToElements();
            var scheduleFieldCount = ScheduleView.Definition.GetFieldCount();
            var paramIdFIeldIndexPair = new Dictionary<ElementId, int>();
            #endregion
            for (int i = 0; i < scheduleFieldCount; i++)
            {
                var fieldId = ScheduleView.Definition.GetField(i).ParameterId;
                var fieldIndex = ScheduleView.Definition.GetField(i).FieldIndex;
                if (fieldId != new ElementId(Convert.ToInt64(-1)))
                {
                    if (!paramIdFIeldIndexPair.ContainsKey(fieldId))
                        paramIdFIeldIndexPair.Add(fieldId, fieldIndex);
                }
                else if (ScheduleView.Definition.GetField(i).IsCombinedParameterField)
                {
                    var combinedParameters = ScheduleView.Definition.GetField(i).GetCombinedParameters().ToList().Select(a => a.ParamId);
                    foreach(var paramId in combinedParameters)
                    {
                        if (!paramIdFIeldIndexPair.ContainsKey(paramId))
                            paramIdFIeldIndexPair.Add(paramId, fieldIndex);
                    }
                }
            }
            //iterating through visible elements and sending parameters to respective parameter processors.
            foreach (var elem in visibleElem)
            {

                var scheduledElement = new ScheduledElement();
                scheduledElement.RowElementId = elem.Id;

                var instanceParameterSet = elem.Parameters.OfType<Parameter>().ToList();
                scheduledElement.ScheduledFields.AddRange(processParameters(elem, paramIdFIeldIndexPair, "Instance", instanceParameterSet));


                var elemSymbol = document.GetElement(elem.GetTypeId());
                var typeParameterSet = elemSymbol.Parameters.OfType<Parameter>().ToList();

                scheduledElement.ScheduledFields.AddRange(processParameters(elem, paramIdFIeldIndexPair, "Type", typeParameterSet));
                ScheduledElements.Add(scheduledElement);

            }
            var dataTable = dataTableBuilder.PrepareTableData(ScheduledElements);

            //TaskDialog.Show("Success", "Read Success");

            return dataTable;
        }
        

        public List<ScheduledField> processParameters (Element elem, Dictionary<ElementId,int> fieldIds, string parameterType, List<Parameter> combinedParameters)
        {
            List<ScheduledField> scheduledFields = combinedParameters                    
                    .Where(p => fieldIds.Keys.Contains(p.Id))
                    .Select(p => new ScheduledField
                    {
                        ParameterElement = p,
                        FieldName = p.Definition.Name,
                        FieldValue = p.AsString() ?? p.AsValueString() ?? string.Empty,
                        SelectedElementId = elem.Id,
                        ParameterType = parameterType,
                        FieldIndex = fieldIds[p.Id]
                    })
                    .ToList();
            return scheduledFields;
        }


    }
}
