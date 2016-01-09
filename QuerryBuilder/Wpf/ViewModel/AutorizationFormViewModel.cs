﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wpf.View;


namespace Wpf.ViewModel
{
    class AutorizationFormViewModel : IDataErrorInfo
    {
        public Action CloseAction { get; set; }
        public ICommand ClickSignInCommand { get; set; }
        public ICommand ClickRegisterCommand { get; set; }

        public AutorizationFormViewModel()
        {
            ClickSignInCommand = new RelayCommand(arg => ClickSignInMethod());
            ClickRegisterCommand = new RelayCommand(arg => ClickRegisterMethod());

        }

        /// <summary>
        /// Вызывает окно регистрации
        /// </summary>
        private void ClickRegisterMethod()
        {
            var windowRegistrationForm = new RegistrationForm();
            windowRegistrationForm.Show();

            CloseAction();
        }

        private void ClickSignInMethod()
        {
            // Верификация аккаунта с базой данных. Вход в аккаунт

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
                        else if (!DataModel.ValidationMethods.EmailValidation(Login))
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