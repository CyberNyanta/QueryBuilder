namespace QueryBuilder.DAL.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConnectionDB")]
    public partial class ConnectionDB
    {
        public ConnectionDB()
        {
            Queries = new HashSet<Query>();
        }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(255)]
        public string ConnectionName { get; set; }

        [Required]
        [StringLength(255)]
        public string ServerName { get; set; }

        [StringLength(255)]
        public string LoginDB { get; set; }

        public Guid? PasswordDB { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConnectionID { get; set; }

        [Required]
        [StringLength(255)]
        public string DatabaseName { get; set; }

        public int Delflag { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }

        public virtual ICollection<Query> Queries { get; set; }
    }
}
