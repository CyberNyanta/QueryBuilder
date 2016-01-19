using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using Wpf.DataModel;
using Wpf.DataModel.Entity;
using Wpf.View;
using Wpf.ViewModel;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
         }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents |*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                var file = ServicesLib.DataTableToExcelExporter.CreateInstance();
                DataTable cc = new DataTable();
                cc.Columns.Add("Column1");
                cc.Rows.Add("1");
                cc.Rows.Add("2");
                file.DataTableExport(cc,filename, "Title");
            }
        }
    }
}
