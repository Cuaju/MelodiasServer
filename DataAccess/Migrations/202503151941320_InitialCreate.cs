namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.userName, unique: true, name: "IX_EmployeeUserName");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", "IX_EmployeeUserName");
            DropTable("dbo.Employees");
        }
    }
}
