using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class PhoneId
    {
        public int Id { get; set; }
        public string FCM { get; set; }
        public int ConventionId { get; set; }
        public virtual Convention Convention { get; set; }
    }
}