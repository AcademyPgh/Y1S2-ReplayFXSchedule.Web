namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Image");
        }
    }
}
