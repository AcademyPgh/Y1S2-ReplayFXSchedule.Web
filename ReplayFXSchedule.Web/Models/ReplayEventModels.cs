 using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class ReplayEvent
    {
        public int Id { get; set; }
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
        public virtual List<ReplayEventType> ReplayEventTypes { get; set; }
    }

    public class ReplayEventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [JsonIgnore]
        public virtual List<ReplayEvent> ReplayEvents { get; set; }
    }

    public class ReplayEventTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class ReplayFXDbContext : DbContext
    {
        public DbSet<ReplayEvent> ReplayEvents { get; set; }
        public DbSet<ReplayEventType> ReplayEventTypes { get; set; }

        public DbSet<ReplayGame> ReplayGames { get; set; }
        public DbSet<ReplayGameLocation> ReplayGameLocations { get; set; }
        public DbSet<ReplayGameType> ReplayGameTypes { get; set; }
    }
}