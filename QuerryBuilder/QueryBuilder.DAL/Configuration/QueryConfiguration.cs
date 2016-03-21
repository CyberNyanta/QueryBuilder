using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class QueryConfiguration : EntityTypeConfiguration<Query>
    {
        public QueryConfiguration()
        {
            ToTable("Queries");
            HasKey(p => p.QueryID);
            Property(p => p.QueryName).HasMaxLength(255);
            Property(p => p.QueryOwner).HasMaxLength(255);
            Property(p => p.ConnectionID).IsRequired();
            Property(p => p.QueryDate).IsRequired().HasColumnType("DateTime");
            HasRequired(p => p.ConnectionDB).WithMany(p => p.Queries).HasForeignKey(p => p.ConnectionID);
        }
    }
}