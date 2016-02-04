using System.Windows;
using System.Windows.Controls;
using Wpf.DataModel;
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
            this.InitializeComponent();
            var vm = new MainWindowFormViewModel();
            this.DataContext = vm;
            MessageBox.Show("For using  all functionality of the application, you have to register or sign-in");

        }

        private void TextBoxSqlQuerry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindowData.SqlQuerry = TextBoxSqlQuerry.Text;
        }

        
    }
}
