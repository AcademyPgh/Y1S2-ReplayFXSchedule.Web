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
    public class Game
    {
        public int Id { get; set; }

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
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
        public bool AtConvention { get; set; }
        [Required]
        [Display(Name = "Game Type")]
        public virtual GameType
           GameType { get; set; }
        [Display(Name = "Game Locations")]
        public virtual List<GameLocation> GameLocations { get; set; }
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
            }
        }

    }

    public class GameLocation
    {
        public int Id { get; set; }
        public string Location { get; set; }
        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<Game> Games { get; set; }
    }

    public class GameType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<Game> Games { get; set; }
    }

}