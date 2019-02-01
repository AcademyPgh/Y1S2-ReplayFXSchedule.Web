﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class ConventionViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string HeaderImage { get; set; } // 640x340
        public string Hashtag { get; set; }
        public string TicketUrl { get; set; }
        public string Url { get; set; }
    }

    public class Sponsor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Image { get; set; } // 200x200ish

        public string ImageUrl
        {
            get
            {
                if (Image != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + ConfigurationManager.AppSettings["AzureFolder"] + @"/" + Image;
                }
                else
                {
                    return Image;
                }
            }
        }

        public virtual Convention Convention { get; set; }
    }

    public class Convention
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [Display(Name = "Header Image")]
        public string HeaderImage { get; set; } // 640x340
        public string Hashtag { get; set; }
        [Display(Name = "Map Image")]
        public string MapImage { get; set; }
        [Display(Name = "Enable in App")]
        public bool EnableInApp { get; set; }
        [Display(Name = "Ticket Url")]
        public string TicketUrl { get; set; }
        public string Url { get; set; }

        public string HeaderImageUrl
        {
            get
            {
                if (HeaderImage != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + ConfigurationManager.AppSettings["AzureFolder"] + @"/" + HeaderImage;
                }
                else
                {
                    return HeaderImage;
                }
            }
        }
        public string MapImageUrl
        {
            get
            {
                if (MapImage != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + ConfigurationManager.AppSettings["AzureFolder"]  + @"/" + MapImage;
                }
                else
                {
                    return MapImage;
                }
            }
        }

        public virtual List<Vendor> Vendors { get; set; }
        public virtual List<GameType> GameTypes { get; set; }
        public virtual List<Game> Games { get; set; }
        public virtual List<GameLocation> GameLocations { get; set; }
        public virtual List<EventType> EventTypes { get; set; }
        public virtual List<Event> Events { get; set; }
        public virtual List<Sponsor> Sponsors { get; set; }

        [JsonIgnore]
        public virtual List<Post> Posts { get; set; }

        [NotMapped]
        public virtual List<Promo> Promos { get
            {
                var promos = Events.Where(e => e.IsPromo == true).Select(e => new Promo()
                {
                    Id = e.Id,
                    ImageUrl = e.PromoImageUrl,
                    Date = e.Date,
                    Type = "event"
                }).ToList();
                return promos;
            }
        }

        [JsonIgnore]
        public virtual List<AppUserPermission> AppUserPermissions { get; set; }
    }
}