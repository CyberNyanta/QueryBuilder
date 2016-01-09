using System.Windows.Input;
using Wpf.View;

namespace Wpf.ViewModel
{
    class MainWindowFormViewModel
    {
        public ICommand ClickAutorizationCommand { get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }

        public MainWindowFormViewModel()
        {
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());

        }
        private void ClickMethodAutorization()
        {
            var windowAutorizationForm = new AutorizationForm();
            windowAutorizationForm.Show();

        }
        private void ClickMethodAddConection()
        {
            var windowConnectionDbForm = new ConnectionDBForm();
            windowConnectionDbForm.Show();
        }
    }
}
