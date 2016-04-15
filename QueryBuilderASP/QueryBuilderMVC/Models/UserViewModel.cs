using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
{
    public class UserViewModel
    {
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = @"Please select user")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        public IEnumerable<UsersListViewModel> Users { get; set; }
    }
}