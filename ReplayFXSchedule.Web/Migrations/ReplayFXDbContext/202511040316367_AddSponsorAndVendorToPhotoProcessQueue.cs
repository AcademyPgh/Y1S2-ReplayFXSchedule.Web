namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSponsorAndVendorToPhotoProcessQueue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhotoProcessQueues", "SponsorId", c => c.Int());
            AddColumn("dbo.PhotoProcessQueues", "VendorId", c => c.Int());
            AlterColumn("dbo.PhotoProcessQueues", "EventId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoProcessQueues", "EventId", c => c.Int(nullable: false));
            DropColumn("dbo.PhotoProcessQueues", "VendorId");
            DropColumn("dbo.PhotoProcessQueues", "SponsorId");
        }
    }
}
