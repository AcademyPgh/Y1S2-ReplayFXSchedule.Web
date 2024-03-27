namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMenuToEventMenus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventMenus", "DisplayName", c => c.String());
            AddColumn("dbo.EventMenus", "IsMenu", c => c.Boolean(nullable: false));
            DropColumn("dbo.EventMenus", "Display");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventMenus", "Display", c => c.String());
            DropColumn("dbo.EventMenus", "IsMenu");
            DropColumn("dbo.EventMenus", "DisplayName");
        }
    }
}
