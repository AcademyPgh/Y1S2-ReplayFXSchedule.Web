namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventLocations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventLocation_Id", c => c.Int());
            AddColumn("dbo.GameLocations", "ShowForGames", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameLocations", "ShowForEvents", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Events", "EventLocation_Id");
            AddForeignKey("dbo.Events", "EventLocation_Id", "dbo.GameLocations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "EventLocation_Id", "dbo.GameLocations");
            DropIndex("dbo.Events", new[] { "EventLocation_Id" });
            DropColumn("dbo.GameLocations", "ShowForEvents");
            DropColumn("dbo.GameLocations", "ShowForGames");
            DropColumn("dbo.Events", "EventLocation_Id");
        }
    }
}
