﻿using System.Data.Entity.ModelConfiguration;
using QueryBuilder.Constants.DbConstants;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.DAL.Configuration
{
#pragma warning disable 0436

    public class UserConfiguration : EntityTypeConfiguration<Models.ApplicationUser>
    {
        public UserConfiguration()
        {
            HasKey(p => p.Email);
            Property(p => p.Email).HasMaxLength(DbLengthString.LongString);
            //Property(p => p.FirstName).IsRequired().HasMaxLength(DbLengthString.LongString);
            //Property(p => p.LastName).IsRequired().HasMaxLength(DbLengthString.LongString);
        }
    }
}