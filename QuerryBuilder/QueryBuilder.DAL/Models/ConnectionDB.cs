using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilder.DAL.Models
{
    [Table("ConnectionDB")]
    public class ConnectionDB
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConnectionID { get; set; }

        [Required]
        public int ConnectionOwner { get; set; }

        [StringLength(255), Required]
        public string ConnectionName { get; set; }

        [StringLength(255), Required]
        public string ServerName { get; set; }

        [StringLength(255)]
        public string LoginDB { get; set; }

        public Guid? PasswordDB { get; set; }

        [StringLength(255), Required]
        public string DatabaseName { get; set; }

        public int Delflag { get; set; }

        [ForeignKey(nameof(ConnectionOwner))]
        public virtual Project Project { get; set; }

        public virtual ICollection<Query> Queries { get; set; }

        public ConnectionDB()
        {
            Queries = new HashSet<Query>();
        }
    }
}
