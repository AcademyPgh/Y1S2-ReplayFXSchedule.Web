namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        IsMenu = c.Boolean(nullable: false),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
            CreateTable(
                "dbo.VendorTypeVendors",
                c => new
                    {
                        VendorType_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VendorType_Id, t.Vendor_Id })
                .ForeignKey("dbo.VendorTypes", t => t.VendorType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.VendorType_Id)
                .Index(t => t.Vendor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VendorTypeVendors", "Vendor_Id", "dbo.Vendors");
            DropForeignKey("dbo.VendorTypeVendors", "VendorType_Id", "dbo.VendorTypes");
            DropForeignKey("dbo.VendorTypes", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.VendorTypeVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.VendorTypeVendors", new[] { "VendorType_Id" });
            DropIndex("dbo.VendorTypes", new[] { "Convention_Id" });
            DropTable("dbo.VendorTypeVendors");
            DropTable("dbo.VendorTypes");
        }
    }
}
