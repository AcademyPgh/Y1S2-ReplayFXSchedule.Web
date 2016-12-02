namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReplayEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Description = c.String(),
                        ExtendedDescription = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReplayEventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReplayEventTypeReplayEvents",
                c => new
                    {
                        ReplayEventType_Id = c.Int(nullable: false),
                        ReplayEvent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReplayEventType_Id, t.ReplayEvent_Id })
                .ForeignKey("dbo.ReplayEventTypes", t => t.ReplayEventType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ReplayEvents", t => t.ReplayEvent_Id, cascadeDelete: true)
                .Index(t => t.ReplayEventType_Id)
                .Index(t => t.ReplayEvent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplayEventTypeReplayEvents", "ReplayEvent_Id", "dbo.ReplayEvents");
            DropForeignKey("dbo.ReplayEventTypeReplayEvents", "ReplayEventType_Id", "dbo.ReplayEventTypes");
            DropIndex("dbo.ReplayEventTypeReplayEvents", new[] { "ReplayEvent_Id" });
            DropIndex("dbo.ReplayEventTypeReplayEvents", new[] { "ReplayEventType_Id" });
            DropTable("dbo.ReplayEventTypeReplayEvents");
            DropTable("dbo.ReplayEventTypes");
            DropTable("dbo.ReplayEvents");
        }
    }
}
