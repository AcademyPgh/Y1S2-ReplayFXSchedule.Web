namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuestList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guests", "Event_Id", c => c.Int());
            CreateIndex("dbo.Guests", "Event_Id");
            AddForeignKey("dbo.Guests", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Guests", "Event_Id", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "Event_Id" });
            DropColumn("dbo.Guests", "Event_Id");
        }
    }
}
