namespace QueryBuilder.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePasswordDb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConnectionDB", "PasswordDB", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConnectionDB", "PasswordDB", c => c.Guid());
        }
    }
}
