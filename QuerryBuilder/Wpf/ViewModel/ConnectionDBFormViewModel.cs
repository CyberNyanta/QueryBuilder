﻿using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Input;

namespace Wpf.ViewModel
{

    class ConnectionDbFormViewModel 
    {
        public string Database { get; set; } = "pubs";
        public string Server { get; set; } = @"(localdb)\ProjectsV12";
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
            MainWindowFormViewModel.UpdateTable(StringConnect(), Database);

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
                return string.Format("Data source = {0}; Initial Catalog = {1}; " +
                                     "User ID = {2}; Password = {3};",Server, Database, User, Password);
                
            }
            
        else
            {
               return $"Data source = {Server}; Initial Catalog = {Database}; Integrated security = {"SSPI"}";
            }
            
        }
    }
}
