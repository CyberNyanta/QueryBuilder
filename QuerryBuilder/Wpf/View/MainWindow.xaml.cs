using System.Windows;
using System.Windows.Controls;
using Wpf.DataModel;
using Wpf.ViewModel;
using System.Windows.Input;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var vm = new MainWindowFormViewModel();
            this.DataContext = vm;
        }

        private void TextBoxSqlQuerry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindowData.SqlQuerry = TextBoxSqlQuerry.Text;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
