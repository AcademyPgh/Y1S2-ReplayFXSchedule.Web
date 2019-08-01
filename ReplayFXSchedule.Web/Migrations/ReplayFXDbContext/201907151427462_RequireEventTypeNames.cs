namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireEventTypeNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventTypes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.EventTypes", "DisplayName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventTypes", "DisplayName", c => c.String());
            AlterColumn("dbo.EventTypes", "Name", c => c.String());
        }
    }
}
