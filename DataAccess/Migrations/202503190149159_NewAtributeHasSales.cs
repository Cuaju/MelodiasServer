namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewAtributeHasSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "HasSales", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "HasSales");
        }
    }
}
