namespace ReplayFXSchedule.Web.Migrations.ReplayFXDbContext
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ReplayFXSchedule.Web.Models.ReplayFXDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ReplayFXDbContext";
        }

        protected override void  Seed(ReplayFXSchedule.Web.Models.ReplayFXDbContext context)
        {
            //string[] eventTypes = { "featured", "games", "competition", "movies", "music", "seminar" };
            //string[] eventTypeDisplays = { "Featured", "Play", "Compete", "Watch", "Listen", "Learn" };

            //for (int i = 0; i < eventTypes.Length; i++)
            //{
                
            //    ReplayEventType tempEventType = new ReplayEventType();
            //    tempEventType.Name = eventTypes[i];
            //    tempEventType.DisplayName = eventTypeDisplays[i];
            //    context.ReplayEventTypes.AddOrUpdate(tempEventType);
            //}
            //context.SaveChanges();

            //string[] gameTypes = { "console", "arcade", "tabletop", "pinball" };

            //for (int i = 0; i < gameTypes.Length; i++)
            //{
            //    ReplayGameType tempGameType = new ReplayGameType();
            //    tempGameType.Name = gameTypes[i];
            //    context.ReplayGameTypes.AddOrUpdate(tempGameType);
            //}
            //context.SaveChanges();
            




            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
