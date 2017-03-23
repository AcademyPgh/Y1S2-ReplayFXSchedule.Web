namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReplayGameLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReplayGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameTitle = c.String(nullable: false),
                        Overview = c.String(),
                        ReleaseDate = c.String(maxLength: 4),
                        Developer = c.String(),
                        Genre = c.String(),
                        Players = c.String(),
                        ReplayGameType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReplayGameTypes", t => t.ReplayGameType_Id, cascadeDelete: true)
                .Index(t => t.ReplayGameType_Id);
            
            CreateTable(
                "dbo.ReplayGameTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReplayGameReplayGameLocations",
                c => new
                    {
                        ReplayGame_Id = c.Int(nullable: false),
                        ReplayGameLocation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReplayGame_Id, t.ReplayGameLocation_Id })
                .ForeignKey("dbo.ReplayGames", t => t.ReplayGame_Id, cascadeDelete: true)
                .ForeignKey("dbo.ReplayGameLocations", t => t.ReplayGameLocation_Id, cascadeDelete: true)
                .Index(t => t.ReplayGame_Id)
                .Index(t => t.ReplayGameLocation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplayGames", "ReplayGameType_Id", "dbo.ReplayGameTypes");
            DropForeignKey("dbo.ReplayGameReplayGameLocations", "ReplayGameLocation_Id", "dbo.ReplayGameLocations");
            DropForeignKey("dbo.ReplayGameReplayGameLocations", "ReplayGame_Id", "dbo.ReplayGames");
            DropIndex("dbo.ReplayGameReplayGameLocations", new[] { "ReplayGameLocation_Id" });
            DropIndex("dbo.ReplayGameReplayGameLocations", new[] { "ReplayGame_Id" });
            DropIndex("dbo.ReplayGames", new[] { "ReplayGameType_Id" });
            DropTable("dbo.ReplayGameReplayGameLocations");
            DropTable("dbo.ReplayGameTypes");
            DropTable("dbo.ReplayGames");
            DropTable("dbo.ReplayGameLocations");
        }
    }
}
