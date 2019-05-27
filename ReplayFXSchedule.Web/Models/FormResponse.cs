using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    // purely for forms in the app for collecting data
    public class UserEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateSubmitted { get; set; }

        public virtual Convention Convention { get; set; }
    }

    public class EmailForm
    {
        public string Email { get; set; }
    }
}