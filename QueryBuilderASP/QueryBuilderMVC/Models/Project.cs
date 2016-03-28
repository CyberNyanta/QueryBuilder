using QueryBuilderMVC.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public string ProjectOwner { get; set; }

        public int Delflag { get; set; }

        public string ProjectDescription { get; set; }
        public virtual ICollection<ConnectionDB> ConnectionDBs { get; set; }

        public virtual ApplicationUser Users { get; set; }
        public virtual ICollection<ProjectsShare> ProjectsShares { get; set; }

        public Project()
        {
            ConnectionDBs = new HashSet<ConnectionDB>();
            ProjectsShares = new HashSet<ProjectsShare>();
        }
    }
}
