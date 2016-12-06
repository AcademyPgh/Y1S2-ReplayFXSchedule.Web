namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReplayEvents", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReplayEvents", "Image");
        }
    }
}
