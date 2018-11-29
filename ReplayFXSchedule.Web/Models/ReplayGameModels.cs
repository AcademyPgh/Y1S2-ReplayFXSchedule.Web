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
    public class ReplayGame
    {
        public int Id { get; set; }
        public virtual ReplayConvention Convention { get; set; }
        [Required]
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }
        [Display(Name = "Game Description")]
        public string Overview { get; set; } 
        [StringLengthAttribute(4, MinimumLength=4, ErrorMessage =  "Date must include 4 digits, eg. 1995")]
        [Display(Name = "Release Year")]
        public string ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Genre { get; set; }
        [Display(Name = "Number of Players")]
        public string Players { get; set; }
        public string Image { get; set; }
        public bool AtReplay { get; set; }
        [Required]
        [Display(Name = "Game Type")]
        public virtual ReplayGameType
           ReplayGameType { get; set; }
        [Display(Name = "Game Locations")]
        public virtual List<ReplayGameLocation> ReplayGameLocations { get; set; }
        public string ImageUrl { get
            {
                if (Image != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + Image;
                }
                else
                {
                    return Image;
                }
            } }

    }

        public class ReplayGameLocation
        {
            public int Id { get; set; }
            public string Location { get; set; }
            public virtual ReplayConvention Convention { get; set; }
            [JsonIgnore]
            public virtual List<ReplayGame> ReplayGames { get; set; }
        }

    public class ReplayGameType
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public virtual ReplayConvention Convention { get; set; }
            [JsonIgnore]
            public virtual List<ReplayGame> ReplayGames { get; set; }
    }

}