namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addURLtoEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "URL");
        }
    }
}
