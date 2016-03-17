using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilder.DAL.Models
{
    [Table("ProjectsShare")]
    public class ProjectsShare
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectID { get; set; }

        [Key, Column(Order = 1), StringLength(255)]
        public string SharedEmail { get; set; }

        public int Delflag { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }
    }
}
