using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilder.DAL.Models
{
    public class Project
    {
#pragma warning disable 0436

        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int Delflag { get; set; }

        public string ProjectDescription { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<ConnectionDB> ConnectionDBs { get; set; }

        public virtual ICollection<ProjectsShare> ProjectsShares { get; set; }

        public Project()
        {
            ConnectionDBs = new HashSet<ConnectionDB>();
            ProjectsShares = new HashSet<ProjectsShare>();
            CreatedDate = DateTime.Now;
        }
    }
}
