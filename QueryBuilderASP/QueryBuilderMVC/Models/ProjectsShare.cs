using System.ComponentModel.DataAnnotations;

namespace QueryBuilderMVC.Models
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
