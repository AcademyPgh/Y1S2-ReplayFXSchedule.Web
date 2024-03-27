namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectGuestsToVendorsAndEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuestEvents",
                c => new
                    {
                        Guest_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Guest_Id, t.Event_Id })
                .ForeignKey("dbo.Guests", t => t.Guest_Id, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .Index(t => t.Guest_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.VendorGuests",
                c => new
                    {
                        Vendor_Id = c.Int(nullable: false),
                        Guest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vendor_Id, t.Guest_Id })
                .ForeignKey("dbo.Vendors", t => t.Vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Guests", t => t.Guest_Id, cascadeDelete: true)
                .Index(t => t.Vendor_Id)
                .Index(t => t.Guest_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VendorGuests", "Guest_Id", "dbo.Guests");
            DropForeignKey("dbo.VendorGuests", "Vendor_Id", "dbo.Vendors");
            DropForeignKey("dbo.GuestEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.GuestEvents", "Guest_Id", "dbo.Guests");
            DropIndex("dbo.VendorGuests", new[] { "Guest_Id" });
            DropIndex("dbo.VendorGuests", new[] { "Vendor_Id" });
            DropIndex("dbo.GuestEvents", new[] { "Event_Id" });
            DropIndex("dbo.GuestEvents", new[] { "Guest_Id" });
            DropTable("dbo.VendorGuests");
            DropTable("dbo.GuestEvents");
        }
    }
}
