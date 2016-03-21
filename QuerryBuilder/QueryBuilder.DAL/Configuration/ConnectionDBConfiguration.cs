using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class ConnectionDbConfiguration : EntityTypeConfiguration<ConnectionDB>
    {
        public ConnectionDbConfiguration()
        {
            ToTable("ConnectionDB");
            HasKey(p => p.ConnectionID);
            Property(p => p.ConnectionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.ConnectionOwner).IsRequired();
            Property(p => p.ConnectionName).IsRequired().HasMaxLength(255);
            Property(p => p.ServerName).IsRequired().HasMaxLength(255);
            Property(p => p.LoginDB).HasMaxLength(255);
            Property(p => p.DatabaseName).IsRequired().HasMaxLength(255);
            HasRequired(p => p.Project).WithMany(p => p.ConnectionDBs).HasForeignKey(p => p.ConnectionOwner);
        }
    }
}