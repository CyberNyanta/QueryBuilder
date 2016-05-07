namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeQueryRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Queries", "ConnectionID", "dbo.ConnectionDB");
            DropIndex("dbo.Queries", new[] { "ConnectionID" });
            AddColumn("dbo.Queries", "ProjectID", c => c.Int(nullable: false));
            CreateIndex("dbo.Queries", "ProjectID");
            AddForeignKey("dbo.Queries", "ProjectID", "dbo.Project", "ProjectID");
            DropColumn("dbo.Queries", "ConnectionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Queries", "ConnectionID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Queries", "ProjectID", "dbo.Project");
            DropIndex("dbo.Queries", new[] { "ProjectID" });
            DropColumn("dbo.Queries", "ProjectID");
            CreateIndex("dbo.Queries", "ConnectionID");
            AddForeignKey("dbo.Queries", "ConnectionID", "dbo.ConnectionDB", "ConnectionID");
        }
    }
}
