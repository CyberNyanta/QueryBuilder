using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wpf.ViewModel.Validations
{
    class TextValidator
    {
        public static bool EmailValidation(string txt)
        {
            string email = @"^([A-Za-z0-9_-]+\.)*[A-Za-z0-9_-]+@[A-Za-z0-9_-]+(\.[A-Za-z0-9_-]+)*\.[A-Za-z]{2,6}$";
            if (Regex.IsMatch(txt, email))
            {
                return true;
            }
            else return false;
        }

        public static bool PassValidation(string txt)
        {
            string pass = @"^(?=.*\d)(?=.*[a-zA-Z0-9])(?=.*[A-Za-z0-9])(?!.*\s).*$";
            if (Regex.IsMatch(txt, pass))
            {
                return true;
            }
            else return false;
        }
    }
}
