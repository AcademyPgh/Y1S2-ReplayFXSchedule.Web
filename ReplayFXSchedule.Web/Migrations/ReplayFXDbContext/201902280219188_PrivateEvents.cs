namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventTypes", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventTypes", "IsMenu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventTypes", "IsMenu");
            DropColumn("dbo.EventTypes", "IsPrivate");
            DropColumn("dbo.Events", "IsPrivate");
        }
    }
}
