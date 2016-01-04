using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AutorizationForm windowAutorizationForm = new AutorizationForm();
            windowAutorizationForm.Show();
        }
        private void ClickMethodAddConection()
        {
            ConnectionDBForm windowConnectionDbForm = new ConnectionDBForm();
            windowConnectionDbForm.Show();
        }
    }
}
