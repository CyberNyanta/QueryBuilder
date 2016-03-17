using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryBuilder.DAL.Models
{
    [Table("Queries")]
    public class Query
    {
        [Key]
        public int QueryID { get; set; }

        [StringLength(255)]
        public string QueryName { get; set; }

        [StringLength(255)]
        public string QueryOwner { get; set; }

        [Required]
        public int ConnectionID { get; set; }

        public string QueryBody { get; set; }

        [DataType(DataType.DateTime), Required]
        public DateTime QueryDate { get; set; }

        public byte[] QueryResult { get; set; }

        public int Delflag { get; set; }

        [ForeignKey(nameof(ConnectionID))]
        public virtual ConnectionDB ConnectionDB { get; set; }
    }
}
