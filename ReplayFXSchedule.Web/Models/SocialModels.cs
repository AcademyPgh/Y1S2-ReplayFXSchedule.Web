using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class PostUpload
    {
        public string Text { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public bool Viewable { get; set; }

        [JsonIgnore]
        public virtual Convention Convention { get; set; }
        [JsonIgnore]
        public virtual AppUser User { get; set; }

        [NotMapped]
        public AppUserView PostedBy { get
            {
                return new AppUserView(){
                    DisplayName = User.DisplayName,
                    Id = User.Id,
                    Image = User.Image,
                    ImageUrl = User.ImageUrl
                };
            }
        }
    }
}