namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaParaProductos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        ProductCode = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        PurchasePrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        SalePrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Category = c.String(nullable: false, maxLength: 50),
                        Brand = c.String(nullable: false, maxLength: 50),
                        Model = c.String(maxLength: 50),
                        Stock = c.Int(nullable: false),
                        Photo = c.String(maxLength: 255),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .Index(t => t.ProductName, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "ProductName" });
            DropTable("dbo.Products");
        }
    }
}
