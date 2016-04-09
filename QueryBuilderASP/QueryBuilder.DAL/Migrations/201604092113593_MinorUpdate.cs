namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationUser", "FirstName", c => c.String(maxLength: 255));
            AlterColumn("dbo.ApplicationUser", "LastName", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUser", "LastName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ApplicationUser", "FirstName", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
