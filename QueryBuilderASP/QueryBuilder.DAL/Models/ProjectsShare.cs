using System.ComponentModel.DataAnnotations;

namespace QueryBuilder.DAL.Models
{
    public class ProjectsShare
    {
        [Key]
        public int ProjectID { get; set; }

        public string SharedEmail { get; set; }

        public int Delflag { get; set; }

        public virtual Project Project { get; set; }
    }
}
