namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ReplayEvents", newName: "Events");
            RenameTable(name: "dbo.ReplayEventTypes", newName: "EventTypes");
            RenameTable(name: "dbo.ReplayGames", newName: "Games");
            RenameTable(name: "dbo.ReplayGameLocations", newName: "GameLocations");
            RenameTable(name: "dbo.ReplayGameTypes", newName: "GameTypes");
            RenameTable(name: "dbo.ReplayVendors", newName: "Vendors");
            RenameTable(name: "dbo.ReplayConventions", newName: "Conventions");

            RenameTable(name: "dbo.ReplayEventTypeReplayEvents", newName: "EventTypeEvents");
            RenameTable(name: "dbo.ReplayGameReplayGameLocations", newName: "GameGameLocations");

            RenameColumn(table: "EventTypeEvents", name: "ReplayEvent_Id", newName: "Event_Id");
            RenameColumn(table: "EventTypeEvents", name: "ReplayEventType_Id", newName: "EventType_Id");
            RenameColumn(table: "GameGameLocations", name: "ReplayGame_Id", newName: "Game_Id");
            RenameColumn(table: "GameGameLocations", name: "ReplayGameLocation_Id", newName: "GameLocation_Id");
            RenameColumn(table: "Games", name: "ReplayGameType_Id", newName: "GameType_Id");
        }

        public override void Down()
        {
            RenameColumn(table: "EventTypeEvents", name: "Event_Id", newName: "ReplayEvent_Id");
            RenameColumn(table: "EventTypeEvents", name: "EventType_Id", newName: "ReplayEventType_Id");
            RenameColumn(table: "GameGameLocations", name: "Game_Id", newName: "ReplayGame_Id");
            RenameColumn(table: "GameGameLocations", name: "GameLocation_Id", newName: "ReplayGameLocation_Id");
            RenameColumn(table: "Games", name: "GameType_Id", newName: "ReplayGameType_Id");


            RenameTable(name: "dbo.Events", newName: "ReplayEvents");
            RenameTable(name: "dbo.EventTypes", newName: "ReplayEventTypes");
            RenameTable(name: "dbo.Games", newName: "ReplayGames");
            RenameTable(name: "dbo.GameLocations", newName: "ReplayGameLocations");
            RenameTable(name: "dbo.GameTypes", newName: "ReplayGameTypes");
            RenameTable(name: "dbo.Vendors", newName: "ReplayVendors");
            RenameTable(name: "dbo.Conventions", newName: "ReplayConventions");

            RenameTable(name: "dbo.EventTypeEvents", newName: "ReplayEventTypeReplayEvents");
            RenameTable(name: "dbo.GameGameLocations", newName: "ReplayGameReplayGameLocations");
        }
    }
}
