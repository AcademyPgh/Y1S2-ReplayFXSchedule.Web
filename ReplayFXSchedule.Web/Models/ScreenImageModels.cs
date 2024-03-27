using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ReplayFXSchedule.Web.Models
{
    public class ScreenImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
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
}