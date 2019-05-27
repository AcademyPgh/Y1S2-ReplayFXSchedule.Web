namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConventionLogo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conventions", "LogoImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conventions", "LogoImage");
        }
    }
}
