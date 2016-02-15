namespace QueryBuilder.DAL.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public User()
        {
            Projects = new HashSet<Project>();
        }

        [Key]
        [StringLength(255)]
        public string Email { get; set; }

        public Guid PasswordHash { get; set; }

        public int Delflag { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
