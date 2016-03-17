using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QueryBuilder.DAL.Models
{
    public class User
    {
        [Key, StringLength(255)]
        public string Email { get; set; }

        public Guid PasswordHash { get; set; }

        public int Delflag { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public User()
        {
            Projects = new HashSet<Project>();
        }
    }
}
