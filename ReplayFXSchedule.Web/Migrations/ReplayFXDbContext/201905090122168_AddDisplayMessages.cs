namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisplayMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Title = c.String(),
                        Text = c.String(),
                        Image = c.String(),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Convention_Id)
                .Index(t => t.Convention_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DisplayMessages", "Convention_Id", "dbo.Conventions");
            DropIndex("dbo.DisplayMessages", new[] { "Convention_Id" });
            DropTable("dbo.DisplayMessages");
        }
    }
}
