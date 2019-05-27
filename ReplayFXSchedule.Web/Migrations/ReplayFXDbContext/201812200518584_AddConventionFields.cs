namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConventionFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conventions", "EnableInApp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Conventions", "TicketUrl", c => c.String());
            AddColumn("dbo.Conventions", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conventions", "Url");
            DropColumn("dbo.Conventions", "TicketUrl");
            DropColumn("dbo.Conventions", "EnableInApp");
        }
    }
}
