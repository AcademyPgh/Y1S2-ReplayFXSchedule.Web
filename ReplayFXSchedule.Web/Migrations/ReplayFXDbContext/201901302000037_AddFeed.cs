namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        PostedOn = c.DateTime(nullable: false),
                        Viewable = c.Boolean(nullable: false),
                        Convention_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.Convention_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "User_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Posts", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.Posts", new[] { "Convention_Id" });
            DropTable("dbo.Posts");
        }
    }
}
