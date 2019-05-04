namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conventions", "AppUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conventions", "AppUrl");
        }
    }
}
