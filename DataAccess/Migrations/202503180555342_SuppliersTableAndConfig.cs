namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuppliersTableAndConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        supplierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 200),
                        PostalCode = c.String(nullable: false, maxLength: 10),
                        City = c.String(nullable: false, maxLength: 50),
                        Country = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.supplierId)
                .Index(t => t.Name, unique: true, name: "IX_SupplierName")
                .Index(t => t.Email, unique: true, name: "IX_SupplierCompanyEmail");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Suppliers", "IX_SupplierCompanyEmail");
            DropIndex("dbo.Suppliers", "IX_SupplierName");
            DropTable("dbo.Suppliers");
        }
    }
}
