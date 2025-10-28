using System.Windows;
using PNCA_SheetLink.SheetLink.ViewModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PNCA_SheetLink.SheetLink.View
{
    public partial class SheetLinkImport : Window
    {
        public SheetLinkImport(Document document, UIDocument uiDocument)
        {
            InitializeComponent();
            this.DataContext = new SheetLinkImportViewModel(document, uiDocument,this);
        }
    }
}