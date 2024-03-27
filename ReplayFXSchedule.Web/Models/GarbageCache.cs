using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class GarbageCache
    {
        public int Id { get; set; }
        public int ConventionId { get; set; }
        public DateTime LastRun { get; set; }
        public string ApiResult { get; set; }
    }
}