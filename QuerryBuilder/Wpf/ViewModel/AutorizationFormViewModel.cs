using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.View;


namespace Wpf.ViewModel
{
    class AutorizationFormViewModel : IDataErrorInfo
    {
        private EntityManager entityManager;

        public Action CloseAction { get; set; }
        public ICommand ClickSignInCommand { get; set; }
        public ICommand ClickRegisterCommand { get; set; }

        public AutorizationFormViewModel()
        {
            entityManager = new EntityManager();
            ClickSignInCommand = new RelayCommand(arg => ClickSignInMethod());
            ClickRegisterCommand = new RelayCommand(arg => ClickRegisterMethod());
        }

       
       /// <summary>
        /// Вызывает окно регистрации
        /// </summary>
        private void ClickRegisterMethod()
        {
            var windowRegistrationForm = new RegistrationForm();
            windowRegistrationForm.ShowDialog();
            CloseAction();
        }

        private void ClickSignInMethod()
        {
            try
            {
                entityManager.LoginUser(Login, Password);
                CloseAction();
            }
            catch (Exception)
            {
                MessageBox.Show(View.Resources.Resource.NotUserLogin);
            }
         }

        public string Login { get; set; }
        public string Password { get; set; }

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
                    case "Login":
                        if (Login == null)
                        {
                            error = "Enter your e-mail";
                        }
                        else if (!ValidationMethods.EmailValidation(Login))
                        {
                            error = "Enter correct e-mail";
                        }

                        break;
                    case "Password":

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