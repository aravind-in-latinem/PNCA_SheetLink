using System;
using System.Data;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PNCA_SheetLink.SheetLink.Model
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RevitDBUpdater

    {
        private Document _document;
        private UIDocument _uiDocument;
        public RevitDBUpdater(Document document, UIDocument uiDocument)
        {
            _document = document;
            _uiDocument = uiDocument;
        }

        public void UpdateRevitDB(DataTable dataTable)
        {
            using (var t = new Transaction(_document))
            {
                t.Start("Import Excel Data");
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {

                        var elemId = new ElementId(Convert.ToInt64(row["ElementId"]));
                        var element = _document.GetElement(elemId);
                        var paramName = row["ParameterName"].ToString();
                        var param = element.LookupParameter(paramName);
                        var setStat = param.Set(row["ValueInTable1"].ToString());

                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", $"Failed to update element. Error: {ex.Message}");
                    }
                }
                t.Commit();

            }
        }
    }
}
