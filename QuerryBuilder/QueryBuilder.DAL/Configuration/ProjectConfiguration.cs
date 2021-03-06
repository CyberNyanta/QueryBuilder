﻿using System.Data.Entity.ModelConfiguration;
using QueryBuilder.Constants.DbConstants;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(p => p.ProjectID);
            Property(p => p.ProjectName).IsRequired().HasMaxLength(DbLengthString.LongString);
            Property(p => p.ProjectOwner).IsRequired().HasMaxLength(DbLengthString.LongString);
            Property(p => p.ProjectDescription).HasMaxLength(DbLengthString.LongString);
            HasRequired(p => p.Users).WithMany(p => p.Projects).HasForeignKey(p => p.ProjectOwner);
        }
    }
}