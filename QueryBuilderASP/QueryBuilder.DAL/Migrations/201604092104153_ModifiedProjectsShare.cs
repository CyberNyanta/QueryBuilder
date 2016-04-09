namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedProjectsShare : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "ProjectOwner", "dbo.ApplicationUser");
            DropIndex("dbo.Project", new[] { "ProjectOwner" });
            DropIndex("dbo.ProjectsShare", new[] { "ProjectID" });
            AddColumn("dbo.ProjectsShare", "UserRole", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectsShare", "ProjectId");
            CreateIndex("dbo.ProjectsShare", "UserId");
            AddForeignKey("dbo.ProjectsShare", "UserId", "dbo.ApplicationUser", "Id");
            DropColumn("dbo.Project", "ProjectOwner");
            DropColumn("dbo.ProjectsShare", "Delflag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectsShare", "Delflag", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "ProjectOwner", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.ProjectsShare", "UserId", "dbo.ApplicationUser");
            DropIndex("dbo.ProjectsShare", new[] { "UserId" });
            DropIndex("dbo.ProjectsShare", new[] { "ProjectId" });
            DropColumn("dbo.ProjectsShare", "UserRole");
            CreateIndex("dbo.ProjectsShare", "ProjectID");
            CreateIndex("dbo.Project", "ProjectOwner");
            AddForeignKey("dbo.Project", "ProjectOwner", "dbo.ApplicationUser", "Id");
        }
    }
}
