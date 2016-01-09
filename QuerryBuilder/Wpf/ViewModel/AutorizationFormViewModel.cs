using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.ViewModel
{
    class AutorizationFormViewModel
    {

    }
    /// <summary>
    /// Класс-валидатор для формы авторизации
    /// </summary>
    class ValidationData : IDataErrorInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Правила валидации
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Login":
                        if ((Login == "a") || (Login == "s"))
                        {
                            //Сообщение ошибки для свойства Login
                            error = "Age can not be less than zero and more than 100";
                        }
                        break;
                    case "Password":
                        //Сообщение ошибки для свойства Password
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