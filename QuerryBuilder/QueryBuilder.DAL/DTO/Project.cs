namespace QueryBuilder.DAL.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {

        public Project()
        {
            ConnectionDBs = new HashSet<ConnectionDB>();
            ProjectsShares = new HashSet<ProjectsShare>();
        }



        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(255)]
        public string ProjectOwner { get; set; }

        public int Delflag { get; set; }

        [StringLength(255)]
        public string ProjectDescription { get; set; }

        public virtual ICollection<ConnectionDB> ConnectionDBs { get; set; }
        
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<ProjectsShare> ProjectsShares { get; set; }
    }
}
