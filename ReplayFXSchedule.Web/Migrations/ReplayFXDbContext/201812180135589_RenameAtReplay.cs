namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAtReplay : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Games", "AtReplay", "AtConvention");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Games", "AtConvention", "AtReplay");
        }
    }
}
