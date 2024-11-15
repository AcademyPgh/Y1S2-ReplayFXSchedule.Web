namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFCM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhoneIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FCM = c.String(),
                        ConventionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.ConventionId, cascadeDelete: true)
                .Index(t => t.ConventionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhoneIds", "ConventionId", "dbo.Conventions");
            DropIndex("dbo.PhoneIds", new[] { "ConventionId" });
            DropTable("dbo.PhoneIds");
        }
    }
}
