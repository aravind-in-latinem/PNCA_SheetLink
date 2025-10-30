using System.Windows;
using PNCA_SheetLink.SheetLink.ViewModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PNCA_SheetLink.SheetLink.View
{
    public partial class SheetLinkWithFormatting : Window
    {
        public SheetLinkWithFormatting(Document document, UIDocument uiDocument)
        {
            InitializeComponent();
            this.DataContext = new SheetLinkWithFormattingViewModel(document, uiDocument,this);
        }
    }
}