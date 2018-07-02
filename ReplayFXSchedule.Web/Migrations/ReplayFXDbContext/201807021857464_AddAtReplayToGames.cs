namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtReplayToGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReplayGames", "AtReplay", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReplayGames", "AtReplay");
        }
    }
}
