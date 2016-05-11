namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQueryHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueriesHistory",
                c => new
                    {
                        QueryHistoryID = c.String(nullable: false, maxLength: 128),
                        QueryID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        QueryDate = c.DateTime(nullable: false),
                        QueryBody = c.String(),
                        Delflag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QueryHistoryID)
                .ForeignKey("dbo.Queries", t => t.QueryID)
                .Index(t => t.QueryID);
            
            DropColumn("dbo.Queries", "QueryOwner");
            DropColumn("dbo.Queries", "QueryDate");
            DropColumn("dbo.Queries", "QueryResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Queries", "QueryResult", c => c.Binary());
            AddColumn("dbo.Queries", "QueryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Queries", "QueryOwner", c => c.Int(nullable: false));
            DropForeignKey("dbo.QueriesHistory", "QueryID", "dbo.Queries");
            DropIndex("dbo.QueriesHistory", new[] { "QueryID" });
            DropTable("dbo.QueriesHistory");
        }
    }
}
