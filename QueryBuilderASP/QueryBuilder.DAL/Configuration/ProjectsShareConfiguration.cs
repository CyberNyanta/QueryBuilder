using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using QueryBuilder.Constants.DbConstants;
using QueryBuilder.DAL.Models;
#pragma warning disable 0436

namespace QueryBuilder.DAL.Configuration
{
    public class ProjectsShareConfiguration : EntityTypeConfiguration<ProjectsShare>
    {
        public ProjectsShareConfiguration()
        {
            ToTable(DbTablesNames.ProjectsShare);
            HasKey(p => new { p.ProjectID, p.SharedEmail });
            Property(p => p.ProjectID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.SharedEmail).HasMaxLength(DbLengthString.LongString);
            HasRequired(p => p.Project).WithMany(p => p.ProjectsShares).HasForeignKey(p => p.ProjectID);
        }
    }
}