using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace PNCA_SheetLink
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "PNCA® BIM Suite";
            string panelNameSheet = "Sheets";

            // Create Tab (ignore if it already exists)
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch { }

            // Check the Existance and Create Panel
            RibbonPanel panel = application.GetRibbonPanels(tabName).FirstOrDefault(p => p.Name == panelNameSheet);
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(tabName, panelNameSheet);
            }

            // DLL Path
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Button
            PushButtonData buttonDataSheetExport = new PushButtonData("SheetExport","Export Excel With EId",assemblyPath,"PNCA_SheetLink.SheetLink.RevitEntryPoint.ScheduleFromElementsExtractor"
            );

            // Icon Path
            Uri uri = new Uri("pack://application:,,,/PNCA_SheetLink;component/SheetLink/Resources/Sheetlink-exporticon-32x32.png",UriKind.Absolute);
            
            // To add Large Image for Button
            BitmapImage iconSheetLink = new BitmapImage(uri);
            buttonDataSheetExport.LargeImage = iconSheetLink;


            // Adding Button to the Tab
            panel.AddItem(buttonDataSheetExport);




            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}