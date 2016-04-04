using QueryBuilder.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<Project> Projects { get; set; }

        public int IdCurrentProject { get; set; }

        [Required(ErrorMessage = @"Please enter project name")]
        public string Name { get; set; }

        [Required(ErrorMessage = @"Enter description")]
        public string Description { get; set; }

        public ConnectionViewModel ConnectionDb { get; set; }

        public IEnumerable<ConnectionDB> _ConnectionDb { get; set; }
    }
}