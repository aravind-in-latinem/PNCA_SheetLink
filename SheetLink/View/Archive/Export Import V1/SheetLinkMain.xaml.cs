using System.Windows;
using PNCA_SheetLink.SheetLink.ViewModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PNCA_SheetLink.SheetLink.View
{
    public partial class SheetLinkMain : Window
    {
        public SheetLinkMain(Document document, UIDocument uiDocument)
        {
            InitializeComponent();
            this.DataContext = new SheetLinkMainViewModel(document, uiDocument,this);
        }
    }
}