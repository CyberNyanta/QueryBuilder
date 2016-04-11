namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyFromUserIdToTableProjectShare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectsShare", "FromUserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectsShare", "FromUserId");
        }
    }
}
