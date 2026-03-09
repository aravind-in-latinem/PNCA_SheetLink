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
        private Document _document;
        private UserLogData _userLogData;

        public ScheduleWithElementIdExporter()
        {
            _logger = new ProgressLoggerViewModel();
            _userLogData = new UserLogData();

        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                
                _userLogData.StartTime = DateTime.Now.ToString("HH:mm:ss");
                _userLogData.AddinName = "ScheduleWithElementIdExporter";
                var uiApplication = commandData.Application;
                var application = uiApplication.Application;
                var uiDocument = uiApplication.ActiveUIDocument;
                _document = uiDocument.Document;
                
                //var scheduleDataFromElements = new ScheduleDataFromElementsExtractor();


                // Create and show your window
                var mainWindow = new SheetLinkExport(_document, uiDocument, _logger);

                // Set owner to Revit window so it stays on top and modal
                System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(mainWindow);
                helper.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

                mainWindow.ShowDialog();

                _userLogData.ProjectName = _document.Title;
                _userLogData.Status = "Success";
                _userLogData.Message = "Schedule exported successfully";
                _userLogData.StopTime = DateTime.Now.ToString("HH:mm:ss");
                UserLogRecorder.SendLog(_userLogData);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to save schedule. Error: {ex.Message}");
                _userLogData.Status = "Fail";
                _userLogData.Message = "Schedule export failed";
                _userLogData.StopTime = DateTime.Now.ToString("HH:mm:ss");
                UserLogRecorder.SendLog(_userLogData);
                return Result.Failed;
            }
            
        }
    }
}
