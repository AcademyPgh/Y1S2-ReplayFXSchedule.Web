namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScreenImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScreenImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScreenImages", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.ScreenImages", new[] { "Convention_Id" });
            DropTable("dbo.ScreenImages");
        }
    }
}
