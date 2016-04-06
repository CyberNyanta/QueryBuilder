using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QueryBuilderMVC.Models
{
    public class ConnectionViewModel
    {
        [Required(ErrorMessage = @"Please enter connection name")]
        public string ConnectionName { get; set; }

        [Required(ErrorMessage = @"Please enter server name")]
        public string ServerName { get; set; }

        [Required(ErrorMessage = @"Please enter login")]
        public string LoginDB { get; set; }

        [Required(ErrorMessage = @"Please enter password")]
        [DataType(DataType.Password)]
        public string PasswordDB { get; set; }

        [Required(ErrorMessage = @"Please enter database name")]
        public string DatabaseName { get; set; }

        public int ConnectionOwner { get; set; }
        public int ConnectionID { get; set; }


    }
}