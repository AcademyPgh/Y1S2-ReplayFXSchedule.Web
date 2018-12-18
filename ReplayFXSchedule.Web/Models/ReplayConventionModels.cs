using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class Convention
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

        public virtual List<Vendor> Vendors { get; set; }
        public virtual List<GameType> GameTypes { get; set; }
        public virtual List<Game> Games { get; set; }
        public virtual List<GameLocation> GameLocations { get; set; }
        public virtual List<EventType> EventTypes { get; set; }
        public virtual List<Event> Events { get; set; }
    }
}