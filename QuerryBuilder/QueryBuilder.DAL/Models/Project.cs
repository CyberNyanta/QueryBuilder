using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilder.DAL.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [StringLength(255), Required]
        public string ProjectName { get; set; }

        [StringLength(255), Required]
        public string ProjectOwner { get; set; }

        public int Delflag { get; set; }

        [StringLength(255)]
        public string ProjectDescription { get; set; }

        public virtual ICollection<ConnectionDB> ConnectionDBs { get; set; }

        [ForeignKey(nameof(ProjectOwner))]
        public virtual User Users { get; set; }

        public virtual ICollection<ProjectsShare> ProjectsShares { get; set; }

        public Project()
        {
            ConnectionDBs = new HashSet<ConnectionDB>();
            ProjectsShares = new HashSet<ProjectsShare>();
        }
    }
}
