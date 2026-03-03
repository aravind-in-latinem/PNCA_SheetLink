using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PNCA_SheetLink.SheetLink.Services;
using PNCA_SheetLink.SheetLink.View;
using PNCA_SheetLink.SheetLink.ViewModel;

namespace PNCA_SheetLink.SheetLink.RevitEntryPoint
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ScheduleWithFormattingExporter : IExternalCommand

    {
        private readonly ILogger _logger;

        public ScheduleWithFormattingExporter()
        {
            _logger = new ProgressLoggerViewModel();
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            var application = uiApplication.Application;
            var uiDocument = uiApplication.ActiveUIDocument;
            var document = uiDocument.Document;


            var sheetLinkWithFormatting = new SheetLinkWithFormatting(document, uiDocument, _logger);

            // Set owner to Revit window so it stays on top and modal
            System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(sheetLinkWithFormatting);
            helper.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            sheetLinkWithFormatting.ShowDialog();           

            return Result.Succeeded;
        }
    }
}
