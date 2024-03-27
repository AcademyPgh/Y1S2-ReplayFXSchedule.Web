namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventMenu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Display = c.String(),
                        Name = c.String(),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
            AddColumn("dbo.EventTypes", "EventMenu_Id", c => c.Int());
            CreateIndex("dbo.EventTypes", "EventMenu_Id");
            AddForeignKey("dbo.EventTypes", "EventMenu_Id", "dbo.EventMenus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventTypes", "EventMenu_Id", "dbo.EventMenus");
            DropForeignKey("dbo.EventMenus", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.EventMenus", new[] { "Convention_Id" });
            DropIndex("dbo.EventTypes", new[] { "EventMenu_Id" });
            DropColumn("dbo.EventTypes", "EventMenu_Id");
            DropTable("dbo.EventMenus");
        }
    }
}
