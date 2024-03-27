namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GarbageCache : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GarbageCaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConventionId = c.Int(nullable: false),
                        LastRun = c.DateTime(nullable: false),
                        ApiResult = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GarbageCaches");
        }
    }
}
