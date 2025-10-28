using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PNCA_SheetLink.SheetLink.ViewModel;

namespace PNCA_SheetLink.SheetLink.View
{
    /// <summary>
    /// Interaction logic for SheetLinkImport.xaml
    /// </summary>
    public partial class SheetLinkImport : Window
    {
        public SheetLinkImport(Document document, UIDocument uIDocument)
        {
            InitializeComponent();
            this.DataContext = new SheetLinkImportViewModel(document,uIDocument,this);
        }
    }
}
