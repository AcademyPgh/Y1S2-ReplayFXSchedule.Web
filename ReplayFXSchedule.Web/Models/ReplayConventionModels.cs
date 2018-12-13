using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class ReplayConvention
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

        public virtual List<ReplayVendor> Vendors { get; set; }
        public virtual List<ReplayGameType> GameTypes { get; set; }
        public virtual List<ReplayGame> Games { get; set; }
        public virtual List<ReplayGameLocation> GameLocations { get; set; }
        public virtual List<ReplayEventType> EventTypes { get; set; }
        public virtual List<ReplayEvent> Events { get; set; }
    }
}