using System;
using System.Windows.Input;
using Wpf.View;

namespace Wpf.ViewModel
{
    class MainWindowFormViewModel
    {
        public Action CloseAction { get; set; }
        public ICommand ClickAutorizationCommand { get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }
        public ICommand ClickCloseCommand { get; set; }

        public MainWindowFormViewModel()
        {
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());
            ClickCloseCommand = new RelayCommand(arg => ClickCloseMethod());
        }

        private void ClickCloseMethod()
        {
            this.CloseAction();
        }

        private void ClickMethodAutorization()
        {
            var windowAutorizationForm = new AutorizationForm();
            windowAutorizationForm.ShowDialog();

        }
        private void ClickMethodAddConection()
        {
            var windowConnectionDbForm = new ConnectionDBForm();
            windowConnectionDbForm.Show();
        }
    }
}
