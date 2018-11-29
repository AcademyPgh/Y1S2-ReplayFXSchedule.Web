namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConvention : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReplayConventions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ReplayEvents", "Convention_Id", c => c.Int());
            AddColumn("dbo.ReplayEventTypes", "Convention_Id", c => c.Int());
            AddColumn("dbo.ReplayGameLocations", "Convention_Id", c => c.Int());
            AddColumn("dbo.ReplayGames", "Convention_Id", c => c.Int());
            AddColumn("dbo.ReplayGameTypes", "Convention_Id", c => c.Int());
            AddColumn("dbo.ReplayVendors", "Convention_Id", c => c.Int());
            CreateIndex("dbo.ReplayEvents", "Convention_Id");
            CreateIndex("dbo.ReplayEventTypes", "Convention_Id");
            CreateIndex("dbo.ReplayGameLocations", "Convention_Id");
            CreateIndex("dbo.ReplayGames", "Convention_Id");
            CreateIndex("dbo.ReplayGameTypes", "Convention_Id");
            CreateIndex("dbo.ReplayVendors", "Convention_Id");
            AddForeignKey("dbo.ReplayEvents", "Convention_Id", "dbo.ReplayConventions", "Id");
            AddForeignKey("dbo.ReplayEventTypes", "Convention_Id", "dbo.ReplayConventions", "Id");
            AddForeignKey("dbo.ReplayGameLocations", "Convention_Id", "dbo.ReplayConventions", "Id");
            AddForeignKey("dbo.ReplayGames", "Convention_Id", "dbo.ReplayConventions", "Id");
            AddForeignKey("dbo.ReplayGameTypes", "Convention_Id", "dbo.ReplayConventions", "Id");
            AddForeignKey("dbo.ReplayVendors", "Convention_Id", "dbo.ReplayConventions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplayVendors", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.ReplayGameTypes", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.ReplayGames", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.ReplayGameLocations", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.ReplayEventTypes", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.ReplayEvents", "Convention_Id", "dbo.ReplayConventions");
            DropIndex("dbo.ReplayVendors", new[] { "Convention_Id" });
            DropIndex("dbo.ReplayGameTypes", new[] { "Convention_Id" });
            DropIndex("dbo.ReplayGames", new[] { "Convention_Id" });
            DropIndex("dbo.ReplayGameLocations", new[] { "Convention_Id" });
            DropIndex("dbo.ReplayEventTypes", new[] { "Convention_Id" });
            DropIndex("dbo.ReplayEvents", new[] { "Convention_Id" });
            DropColumn("dbo.ReplayVendors", "Convention_Id");
            DropColumn("dbo.ReplayGameTypes", "Convention_Id");
            DropColumn("dbo.ReplayGames", "Convention_Id");
            DropColumn("dbo.ReplayGameLocations", "Convention_Id");
            DropColumn("dbo.ReplayEventTypes", "Convention_Id");
            DropColumn("dbo.ReplayEvents", "Convention_Id");
            DropTable("dbo.ReplayConventions");
        }
    }
}
