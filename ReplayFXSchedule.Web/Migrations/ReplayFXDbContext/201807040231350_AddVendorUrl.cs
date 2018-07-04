namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReplayVendors", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReplayVendors", "Url");
        }
    }
}
