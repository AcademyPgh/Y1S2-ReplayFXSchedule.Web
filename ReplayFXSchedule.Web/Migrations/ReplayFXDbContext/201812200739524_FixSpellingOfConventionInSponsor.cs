namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSpellingOfConventionInSponsor : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Sponsors", name: "Covention_Id", newName: "Convention_Id");
            RenameIndex(table: "dbo.Sponsors", name: "IX_Covention_Id", newName: "IX_Convention_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Sponsors", name: "IX_Convention_Id", newName: "IX_Covention_Id");
            RenameColumn(table: "dbo.Sponsors", name: "Convention_Id", newName: "Covention_Id");
        }
    }
}
