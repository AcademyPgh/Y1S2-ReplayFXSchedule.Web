namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromoImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "PromoImage", c => c.String());
            AddColumn("dbo.Events", "IsPromo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsPromo");
            DropColumn("dbo.Events", "PromoImage");
        }
    }
}
