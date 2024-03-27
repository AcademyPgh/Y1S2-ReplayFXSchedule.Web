namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalEventMenu : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventTypeEvents", newName: "EventEventTypes");
            DropPrimaryKey("dbo.EventEventTypes");
            AddPrimaryKey("dbo.EventEventTypes", new[] { "Event_Id", "EventType_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.EventEventTypes");
            AddPrimaryKey("dbo.EventEventTypes", new[] { "EventType_Id", "Event_Id" });
            RenameTable(name: "dbo.EventEventTypes", newName: "EventTypeEvents");
        }
    }
}
