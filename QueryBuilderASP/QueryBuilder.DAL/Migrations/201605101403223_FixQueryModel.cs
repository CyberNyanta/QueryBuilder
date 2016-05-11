namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixQueryModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Queries", "QueryOwner", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Queries", "QueryOwner", c => c.String(maxLength: 255));
        }
    }
}
