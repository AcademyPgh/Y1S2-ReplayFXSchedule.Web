namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        DisplayName = c.String(),
                        Auth0 = c.String(),
                        isSuperAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppUserPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserRole = c.Int(nullable: false),
                        AppUser_Id = c.Int(),
                        Convention_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.ReplayConventions", t => t.Convention_Id)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Convention_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserPermissions", "Convention_Id", "dbo.ReplayConventions");
            DropForeignKey("dbo.AppUserPermissions", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.AppUserPermissions", new[] { "Convention_Id" });
            DropIndex("dbo.AppUserPermissions", new[] { "AppUser_Id" });
            DropTable("dbo.AppUserPermissions");
            DropTable("dbo.AppUsers");
        }
    }
}
