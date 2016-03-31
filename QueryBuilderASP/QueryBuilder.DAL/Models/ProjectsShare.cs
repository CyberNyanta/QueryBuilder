using System.ComponentModel.DataAnnotations;
#pragma warning disable 0436

namespace QueryBuilder.DAL.Models
{
    public class ProjectsShare
    {
        public int ProjectID { get; set; }

        public string SharedEmail { get; set; }

        public int Delflag { get; set; }

        public virtual Project Project { get; set; }
    }
}
