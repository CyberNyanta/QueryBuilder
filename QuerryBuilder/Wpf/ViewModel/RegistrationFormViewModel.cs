using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.ViewModel
{
    class RegistrationFormViewModel
    {
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value;}
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _pass;

        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }

        private string _confirmPass;

        public string ConfirmPass
        {
            get { return _confirmPass; }
            set { _confirmPass = value; }
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


    }
}
