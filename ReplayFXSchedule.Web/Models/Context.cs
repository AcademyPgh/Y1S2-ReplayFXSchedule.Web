using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class ReplayFXDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameLocation> GameLocations { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestType> GuestTypes { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<Convention> Conventions { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserPermission> AppUserPermissions { get; set; }

        public DbSet<UserEmail> UserEmails { get; set; }
        public DbSet<DisplayMessage> DisplayMessages { get; set; }

        public DbSet<ScreenImage> ScreenImages { get; set; }

        public DbSet<GarbageCache> Cache { get; set; }
        public System.Data.Entity.DbSet<ReplayFXSchedule.Web.Models.EventMenu> EventMenus { get; set; }
    }
}