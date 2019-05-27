namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLocationString : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Location", c => c.String());
        }
    }
}
