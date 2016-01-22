﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.ViewModel.Command;

namespace Wpf.ViewModel
{
    class AddUsersToEmailWindowViewModel : Notifier, IDataErrorInfo
    {
        private string _projectName;
        public string Email { get; set; }
        public string Title { get; set; }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        public ICommand ClickSendMailCommand { get; set; }
        public Action CloseAction { get; set; }

        public AddUsersToEmailWindowViewModel()
        {
            ClickSendMailCommand = new RelayCommand(arg => ClickMethodSendEmail());
            ProjectName = MainWindowData.ProjectName;
            OnPropertyChanged("ProjectName");
            
        }

        private void ClickMethodSendEmail()
        {
            try
            {
                var mail = ServicesLib.SmtpMailer.Instance();
                mail.SendMail(Email, Title, ProjectName);
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("Mail sent", "", button, icon);
                this.CloseAction();
            }
            catch (Exception)
            {

                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBox.Show("Mail didn't send", "", button, icon);
            }

        }

        public string this[string columnName]
        {

            get
            {

                var error = string.Empty;
                switch (columnName)
                {
                    case "Email":
                        if (Email == null)
                        {
                            error = "Enter your e-mail";
                        }
                        else if (!ValidationMethods.EmailValidation(Email))
                        {
                            error = "Enter correct e-mail";
                        }

                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
