using System.Data.Entity.ModelConfiguration;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(p => p.ProjectID);
            Property(p => p.ProjectName).IsRequired().HasMaxLength(255);
            Property(p => p.ProjectOwner).IsRequired().HasMaxLength(255);
            Property(p => p.ProjectDescription).HasMaxLength(255);
            HasRequired(p => p.Users).WithMany(p => p.Projects).HasForeignKey(p => p.ProjectOwner);
        }
    }
}