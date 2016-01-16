using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Wpf.DataModel;

namespace Wpf.ViewModel
{

    class ConnectionDbFormViewModel 
    {
        public string Database { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool WindowsAutorizeted { get; set; }
        public ICommand ClickTestConnectionCommand { get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }

        public Action CloseAction { get; set; }

        public ConnectionDbFormViewModel()
        {
            ClickTestConnectionCommand = new RelayCommand(arg => ClickMethodTestConection());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());
            
        }

        private void ClickMethodAddConection()
        {
            //Метод добавления подключения
            CloseAction();
        }

        private void ClickMethodTestConection()
        {
            MessageBox.Show(ConnectionDatabase.TestConnectDb(StringConnect()) ? "True" : "False");
        }
        
        private string StringConnect()
        {
            if (!WindowsAutorizeted)
            {
                return $"Data source = {Server}; Initial Catalog = {Database}; User ID = {User}; Password = {Password};";
            }
            
        else
            {
               return $"Data source = {Server}; Initial Catalog = {Database}; Integrated security = {"SSPI"}";
            }
            
        }
    }
}
