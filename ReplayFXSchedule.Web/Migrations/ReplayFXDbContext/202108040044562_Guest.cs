namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Guest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ExtendedDescription = c.String(),
                        Image = c.String(),
                        Url = c.String(),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
            CreateTable(
                "dbo.GuestTypes",
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
                "dbo.GuestTypeGuests",
                c => new
                    {
                        GuestType_Id = c.Int(nullable: false),
                        Guest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GuestType_Id, t.Guest_Id })
                .ForeignKey("dbo.GuestTypes", t => t.GuestType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Guests", t => t.Guest_Id, cascadeDelete: true)
                .Index(t => t.GuestType_Id)
                .Index(t => t.Guest_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GuestTypeGuests", "Guest_Id", "dbo.Guests");
            DropForeignKey("dbo.GuestTypeGuests", "GuestType_Id", "dbo.GuestTypes");
            DropForeignKey("dbo.GuestTypes", "Convention_Id", "dbo.Conventions");
            DropForeignKey("dbo.Guests", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.GuestTypeGuests", new[] { "Guest_Id" });
            DropIndex("dbo.GuestTypeGuests", new[] { "GuestType_Id" });
            DropIndex("dbo.GuestTypes", new[] { "Convention_Id" });
            DropIndex("dbo.Guests", new[] { "Convention_Id" });
            DropTable("dbo.GuestTypeGuests");
            DropTable("dbo.GuestTypes");
            DropTable("dbo.Guests");
        }
    }
}
