namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateConferenceForApp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sponsors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        Image = c.String(),
                        Covention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conventions", t => t.Covention_Id)
                .Index(t => t.Covention_Id);
            
            AddColumn("dbo.Conventions", "HeaderImage", c => c.String());
            AddColumn("dbo.Conventions", "Hashtag", c => c.String());
            AddColumn("dbo.Conventions", "MapImage", c => c.String());
            AddColumn("dbo.GameTypes", "HeaderImage", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sponsors", "Covention_Id", "dbo.Conventions");
            DropIndex("dbo.Sponsors", new[] { "Covention_Id" });
            DropColumn("dbo.GameTypes", "HeaderImage");
            DropColumn("dbo.Conventions", "MapImage");
            DropColumn("dbo.Conventions", "Hashtag");
            DropColumn("dbo.Conventions", "HeaderImage");
            DropTable("dbo.Sponsors");
        }
    }
}
