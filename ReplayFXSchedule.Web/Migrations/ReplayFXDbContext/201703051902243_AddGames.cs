namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGames : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReplayGames", "ReplayGameType_Id", "dbo.ReplayGameTypes");
            DropIndex("dbo.ReplayGames", new[] { "ReplayGameType_Id" });
            AlterColumn("dbo.ReplayGames", "GameTitle", c => c.String(nullable: false));
            AlterColumn("dbo.ReplayGames", "ReleaseDate", c => c.String(maxLength: 4));
            AlterColumn("dbo.ReplayGames", "ReplayGameType_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ReplayGames", "ReplayGameType_Id");
            AddForeignKey("dbo.ReplayGames", "ReplayGameType_Id", "dbo.ReplayGameTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplayGames", "ReplayGameType_Id", "dbo.ReplayGameTypes");
            DropIndex("dbo.ReplayGames", new[] { "ReplayGameType_Id" });
            AlterColumn("dbo.ReplayGames", "ReplayGameType_Id", c => c.Int());
            AlterColumn("dbo.ReplayGames", "ReleaseDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReplayGames", "GameTitle", c => c.String());
            CreateIndex("dbo.ReplayGames", "ReplayGameType_Id");
            AddForeignKey("dbo.ReplayGames", "ReplayGameType_Id", "dbo.ReplayGameTypes", "Id");
        }
    }
}
