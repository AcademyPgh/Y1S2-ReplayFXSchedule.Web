namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackingUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conventions", "TrackingUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conventions", "TrackingUrl");
        }
    }
}
