 using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Extended Description")]
        public string ExtendedDescription { get; set; }
        public string Location {
            get
            {
                if(EventLocation == null)
                {
                    return "No Location";
                }
                else
                {
                    return EventLocation.Location;
                }
            }
        }
        public  virtual GameLocation EventLocation { get; set; }
        public string Image { get; set; }
        public string PromoImage { get; set; }
        public bool IsPromo { get; set; }
        public bool IsPrivate { get; set; }
        public string URL { get; set; } // add if to get to check for https

        [Display(Name = "Event Type")]
        public virtual List<EventType> EventTypes { get; set; }
        public string StartTime12
        {
            get {
                if (DateTime.TryParse(StartTime, out DateTime output))
                {
                    return output.ToString("hh\\:mm tt");
                }
                return null;
            }
        }

        public string EndTime12
        {
            get {
                if (DateTime.TryParse(EndTime, out DateTime output))
                {
                    return output.ToString("hh\\:mm tt");
                }
                return null;
            }
        }
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
        public string PromoImageUrl
        {
            get
            {
                if (PromoImage != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + ConfigurationManager.AppSettings["AzureFolder"] + @"/" + PromoImage;
                }
                else
                {
                    return PromoImage;
                }
            }
        }
    }

    public class EventType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsMenu { get; set; }
        public virtual EventMenu EventMenu {get;set;}

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<Event> Events { get; set; }
    }

    public class EventMenu
    {
        public int Id { get; set; }
        public string Display { get; set; }
        public string Name { get; set; }
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual List<EventType> EventTypes { get; set; }
    }

    public class EventTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class Promo
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}