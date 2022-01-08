using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class Guest
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Extended Description")]
        public string ExtendedDescription { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public virtual List<GuestType> GuestTypes { get; set; }
        public virtual List<Event> Events { get; set; }
        public virtual List<Vendor> Vendors { get; set; }
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
    }

    public class GuestType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsMenu { get; set; }

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<Guest> Guests { get; set; }
    }

    public class GuestView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GuestTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}