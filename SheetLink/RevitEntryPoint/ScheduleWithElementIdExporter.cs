using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PNCA_SheetLink.SheetLink.Model;
using PNCA_SheetLink.SheetLink.Services;
using PNCA_SheetLink.SheetLink.View;
using PNCA_SheetLink.SheetLink.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PNCA_SheetLink.SheetLink.RevitEntryPoint
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ScheduleWithElementIdExporter : IExternalCommand

    {
        private ILogger _logger;

        private Document doc;
        private string Status;
        private DateTime nowStart;
        private DateTime nowStop;

        public ScheduleWithElementIdExporter()
        {
            _logger = new ProgressLoggerViewModel();
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
            nowStart = DateTime.Now;

            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;
            //var scheduleDataFromElements = new ScheduleDataFromElementsExtractor();
                         

            // Create and show your window
            var mainWindow = new SheetLinkExport(document, uiDocument, _logger);

            // Set owner to Revit window so it stays on top and modal
            System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(mainWindow);
            helper.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            mainWindow.ShowDialog();

            doc = document;
            Status = "Sucess";

                nowStop = DateTime.Now;
                RecordLog.SendLog(doc.Title, "SheetExport", "Success", "Sheets exported successfully", nowStart.ToString(), nowStop.ToString());
            return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to save schedule. Error: {ex.Message}");
                Status = "Fail";
                nowStop = DateTime.Now;
                RecordLog.SendLog(doc.Title, "SheetExport", "Success", "Sheets exported successfully", nowStart.ToString(), nowStop.ToString());
                return Result.Failed;
            }
            
        }
    }
}
