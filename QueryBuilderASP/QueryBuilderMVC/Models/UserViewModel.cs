using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = @"Please enter id")]
        public string Id { get; set; }

        [Required(ErrorMessage = @"Please enter user name")]
        public string UserName { get; set; }

        public IEnumerable<UsersListViewModel> Users { get; set; }
    }
}