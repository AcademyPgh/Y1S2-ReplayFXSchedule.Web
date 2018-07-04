using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class ReplayVendor
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Extended Description")]
        public string ExtendedDescription { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }

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
}