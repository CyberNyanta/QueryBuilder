using System.Data.Entity.ModelConfiguration;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(p => p.Email);
            Property(p => p.Email).HasMaxLength(255);
            Property(p => p.FirstName).IsRequired().HasMaxLength(255);
            Property(p => p.LastName).IsRequired().HasMaxLength(255);
        }
    }
}