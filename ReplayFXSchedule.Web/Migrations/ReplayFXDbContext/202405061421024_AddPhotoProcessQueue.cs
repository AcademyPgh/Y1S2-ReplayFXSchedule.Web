namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoProcessQueue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhotoProcessQueues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        URL = c.String(),
                        Status = c.Int(nullable: false),
                        Error = c.String(),
                        Created = c.DateTime(nullable: false),
                        Processed = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhotoProcessQueues");
        }
    }
}
