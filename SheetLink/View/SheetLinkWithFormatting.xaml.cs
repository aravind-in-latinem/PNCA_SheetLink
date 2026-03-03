using System;
using System.Windows;
using PNCA_SheetLink.SheetLink.ViewModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Input;
using PNCA_SheetLink.SheetLink.Services;

namespace PNCA_SheetLink.SheetLink.View
{
    public partial class SheetLinkWithFormatting : Window
    {
        public SheetLinkWithFormatting(Document document, UIDocument uiDocument, ILogger progressLogger)
        {
            InitializeComponent();
            this.DataContext = new SheetLinkWithFormattingViewModel(document, uiDocument,this, progressLogger);
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}