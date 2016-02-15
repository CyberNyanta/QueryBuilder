namespace QueryBuilder.DAL.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Queries")]
    public partial class Query
    {
        [Key]
        public int QueryID { get; set; }


        [StringLength(255)]
        public string QueryName { get; set; }

        [StringLength(255)]
        public string QueryOwner { get; set; }

        [Required]
        public int ConnectionID { get; set; }

        [StringLength(255)]
        public string ResultBody { get; set; }


        public int Delflag { get; set; }

        [ForeignKey(nameof(ConnectionID))]
        public virtual ConnectionDB ConnectionDB { get; set; }
    }
}
