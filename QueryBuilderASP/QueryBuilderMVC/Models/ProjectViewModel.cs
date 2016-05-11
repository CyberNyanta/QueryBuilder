using QueryBuilder.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<ProjectsListViewModel> Projects { get; set; }

        public int IdCurrentProject { get; set; }

        [Required(ErrorMessage = @"Please enter project name")]
        [MaxLength(16, ErrorMessage = "Limit length string for 16 letters")]
        public string Name { get; set; }

        [Required(ErrorMessage = @"Enter description")]
        [MaxLength(150, ErrorMessage = "Limit length string for 150 symbols")]
        public string Description { get; set; }

        public IEnumerable<ConnectionsListViewModel> ConnectionDbs { get; set; }

		public IEnumerable<QueriesListViewModel> Queries { get; set; }

		//public IEnumerable<ConnectionDB> _ConnectionDb { get; set; }
	}
}