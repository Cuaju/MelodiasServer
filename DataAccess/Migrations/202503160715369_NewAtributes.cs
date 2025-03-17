namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewAtributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Name", c => c.String());
            AddColumn("dbo.Employees", "Surnames", c => c.String());
            AddColumn("dbo.Employees", "Phone", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Email", c => c.String());
            AddColumn("dbo.Employees", "Address", c => c.String());
            AddColumn("dbo.Employees", "ZipCode", c => c.String());
            AddColumn("dbo.Employees", "City", c => c.String());
            AddColumn("dbo.Employees", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Password");
            DropColumn("dbo.Employees", "City");
            DropColumn("dbo.Employees", "ZipCode");
            DropColumn("dbo.Employees", "Address");
            DropColumn("dbo.Employees", "Email");
            DropColumn("dbo.Employees", "Phone");
            DropColumn("dbo.Employees", "Surnames");
            DropColumn("dbo.Employees", "Name");
        }
    }
}
