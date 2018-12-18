 using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ReplayFXSchedule.Web.Models
{
    public class Event
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name ="Date")]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        public string Description { get; set; }
        [Display(Name = "Extended Description")]
        public string ExtendedDescription { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }

        [Display(Name = "Event Type")]
        public virtual List<EventType> ReplayEventTypes { get; set; }
        public string StartTime12
        {
            get { return DateTime.Parse(StartTime).ToString("hh\\:mm tt"); }
        }

        public string EndTime12
        {
            get { return DateTime.Parse(EndTime).ToString("hh\\:mm tt"); }
        }
        public string ImageUrl
        {
            get
            {
                if (Image != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + Image;
                }
                else
                {
                    return Image;
                }
            }
        }
    }

    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<Event> ReplayEvents { get; set; }
    }

    public class ReplayEventTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class ReplayFXDbContext : DbContext
    {
        public DbSet<Event> ReplayEvents { get; set; }
        public DbSet<EventType> ReplayEventTypes { get; set; }

        public DbSet<Game> ReplayGames { get; set; }
        public DbSet<GameLocation> ReplayGameLocations { get; set; }
        public DbSet<GameType> ReplayGameTypes { get; set; }
        public DbSet<Vendor> ReplayVendors { get; set; }
        public DbSet<Convention> ReplayConventions { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}