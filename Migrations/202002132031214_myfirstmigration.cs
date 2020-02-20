namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myfirstmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BrandID);
            
            CreateTable(
                "dbo.MakeupProducts",
                c => new
                    {
                        MakeupProductID = c.Int(nullable: false, identity: true),
                        MakeupProductName = c.String(),
                        ProductionDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Notes = c.String(),
                        BrandID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MakeupProductID)
                .ForeignKey("dbo.Brands", t => t.BrandID, cascadeDelete: true)
                .Index(t => t.BrandID);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreID = c.Int(nullable: false, identity: true),
                        StoreName = c.String(),
                        StoreAddress = c.String(),
                        StoreOwner = c.String(),
                    })
                .PrimaryKey(t => t.StoreID);
            
            CreateTable(
                "dbo.StoreMakeupProducts",
                c => new
                    {
                        Store_StoreID = c.Int(nullable: false),
                        MakeupProduct_MakeupProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Store_StoreID, t.MakeupProduct_MakeupProductID })
                .ForeignKey("dbo.Stores", t => t.Store_StoreID, cascadeDelete: true)
                .ForeignKey("dbo.MakeupProducts", t => t.MakeupProduct_MakeupProductID, cascadeDelete: true)
                .Index(t => t.Store_StoreID)
                .Index(t => t.MakeupProduct_MakeupProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreMakeupProducts", "MakeupProduct_MakeupProductID", "dbo.MakeupProducts");
            DropForeignKey("dbo.StoreMakeupProducts", "Store_StoreID", "dbo.Stores");
            DropForeignKey("dbo.MakeupProducts", "BrandID", "dbo.Brands");
            DropIndex("dbo.StoreMakeupProducts", new[] { "MakeupProduct_MakeupProductID" });
            DropIndex("dbo.StoreMakeupProducts", new[] { "Store_StoreID" });
            DropIndex("dbo.MakeupProducts", new[] { "BrandID" });
            DropTable("dbo.StoreMakeupProducts");
            DropTable("dbo.Stores");
            DropTable("dbo.MakeupProducts");
            DropTable("dbo.Brands");
        }
    }
}
