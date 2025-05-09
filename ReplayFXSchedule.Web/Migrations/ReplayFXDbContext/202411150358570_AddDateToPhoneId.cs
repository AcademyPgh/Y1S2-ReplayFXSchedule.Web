namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateToPhoneId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhoneIds", "LastContact", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhoneIds", "LastContact");
        }
    }
}
