using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.ViewModel.Command;

namespace Wpf.ViewModel
{
   class RegistrationFormViewModel : IDataErrorInfo
    {
        public Action CloseAction { get; set; }

        private EntityManager entityManager;
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICommand ClickCloseCommand { get; set; }
        public ICommand ClickRegisterCommand { get; set; }

        public RegistrationFormViewModel()
        {
            entityManager = new EntityManager();
            ClickCloseCommand = new RelayCommand(arg => ClickCloseMethod());
            ClickRegisterCommand = new RelayCommand(arg => ClickRegisterMethod());
        }

        private void ClickRegisterMethod()
        { 
            try
            {
                if (Password != ConfirmPassword)
                {
                    MessageBox.Show(View.Resources.Resource.EqualsPasswords);
                }
               entityManager.RegistrationUser(FirstName, LastName, Email, ConfirmPassword);
               CloseAction();
                MessageBox.Show(String.Format(View.Resources.Resource.RegistrationComplete, FirstName),"Registration complete!", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show(View.Resources.Resource.NotUserLogin);
            }
            
        }

        private void ClickCloseMethod()
        {
            CloseAction();
        }

        /// <summary>
        /// Валидация и сообщения об ошибках
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
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
                    case "FirstName":
                        if (FirstName == null)
                        {
                            error = "Enter your first name";
                        }
                        else if (!ValidationMethods.NameValidation(FirstName))
                        {
                            error = "Enter correct first name";
                        }
                        break;
                    case "LastName":
                        if (LastName == null | LastName == string.Empty)
                        {
                            error = "Enter your last name";
                        }
                        else if (!ValidationMethods.NameValidation(LastName))
                        {
                            error = "Enter correct last name";
                        }

                        break;
                    case "Password":
                        if (LastName == null)
                        {
                            error = "Enter your last name";
                        }
                        else if (!ValidationMethods.NameValidation(LastName))
                        {
                            error = "Enter correct last name";
                            MessageBox.Show("Wrong password");
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

