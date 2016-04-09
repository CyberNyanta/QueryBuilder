namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialManualMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConnectionDB",
                c => new
                    {
                        ConnectionID = c.Int(nullable: false, identity: true),
                        ConnectionOwner = c.Int(nullable: false),
                        ConnectionName = c.String(nullable: false, maxLength: 255),
                        ServerName = c.String(nullable: false, maxLength: 255),
                        LoginDB = c.String(maxLength: 255),
                        PasswordDB = c.Guid(),
                        DatabaseName = c.String(nullable: false, maxLength: 255),
                        Delflag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionID)
                .ForeignKey("dbo.Project", t => t.ConnectionOwner)
                .Index(t => t.ConnectionOwner);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 255),
                        ProjectOwner = c.String(nullable: false, maxLength: 128),
                        Delflag = c.Int(nullable: false),
                        ProjectDescription = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.ApplicationUser", t => t.ProjectOwner)
                .Index(t => t.ProjectOwner);
            
            CreateTable(
                "dbo.ProjectsShare",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Delflag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.UserId })
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Delflag = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(maxLength: 255),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Queries",
                c => new
                    {
                        QueryID = c.Int(nullable: false, identity: true),
                        QueryName = c.String(maxLength: 255),
                        QueryOwner = c.String(maxLength: 255),
                        ConnectionID = c.Int(nullable: false),
                        QueryBody = c.String(),
                        QueryDate = c.DateTime(nullable: false),
                        QueryResult = c.Binary(),
                        Delflag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QueryID)
                .ForeignKey("dbo.ConnectionDB", t => t.ConnectionID)
                .Index(t => t.ConnectionID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Queries", "ConnectionID", "dbo.ConnectionDB");
            DropForeignKey("dbo.ConnectionDB", "ConnectionOwner", "dbo.Project");
            DropForeignKey("dbo.Project", "ProjectOwner", "dbo.ApplicationUser");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.ProjectsShare", "ProjectID", "dbo.Project");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Queries", new[] { "ConnectionID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectsShare", new[] { "ProjectID" });
            DropIndex("dbo.Project", new[] { "ProjectOwner" });
            DropIndex("dbo.ConnectionDB", new[] { "ConnectionOwner" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Queries");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.ProjectsShare");
            DropTable("dbo.Project");
            DropTable("dbo.ConnectionDB");
        }
    }
}
