using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class ProjectsShareConfiguration : EntityTypeConfiguration<ProjectsShare>
    {
        public ProjectsShareConfiguration()
        {
            ToTable("ProjectsShare");
            HasKey(p => new { p.ProjectID, p.SharedEmail });
            Property(p => p.ProjectID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.SharedEmail).HasMaxLength(255);
            HasRequired(p => p.Project).WithMany(p => p.ProjectsShares).HasForeignKey(p => p.ProjectID);
        }
    }
}