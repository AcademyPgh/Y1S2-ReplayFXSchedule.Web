using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace ReplayFXSchedule.Web.Models
{
    public class ReplayGame
    {
        public int Id { get; set; }
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }
        [Display(Name = "Game Description")]
        public string Overview { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Genres { get; set; }
        [Display(Name = "Number of Players")]
        public string Players { get; set; }
        [Display(Name = "Game Locations")]
        public List<ReplayGameLocation> ReplayGameLocations { get; set; }
        public List<ReplayGameType> ReplayGameTypes { get; set; }




        public class ReplayGameLocation
        {
            public int Id { get; set; }
            public string Location { get; set; }
            [JsonIgnore]
            public virtual List<ReplayGame> ReplayGames { get; set; }
        }

    public class ReplayGameType
    {
            public int Id { get; set; }
            public string Type { get; set; }
            [JsonIgnore]
            public virtual List<ReplayGame> ReplayGames { get; set; }
    }

        public class ReplayFXDbContext : DbContext
        {
            public DbSet<ReplayGame> ReplayGames { get; set; }
            public DbSet<ReplayGameLocation> ReplayGameLocations { get; set; }
            public DbSet<ReplayGameType> ReplayGameTypes { get; set; }
        }
    }

}