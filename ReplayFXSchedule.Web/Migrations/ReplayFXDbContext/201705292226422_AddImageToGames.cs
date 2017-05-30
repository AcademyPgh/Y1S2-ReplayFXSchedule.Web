namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReplayGames", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReplayGames", "Image");
        }
    }
}
