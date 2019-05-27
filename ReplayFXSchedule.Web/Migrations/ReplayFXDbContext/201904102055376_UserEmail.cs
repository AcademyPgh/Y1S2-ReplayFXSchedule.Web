namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        DateSubmitted = c.DateTime(nullable: false),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserEmails", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.UserEmails", new[] { "Convention_Id" });
            DropTable("dbo.UserEmails");
        }
    }
}
