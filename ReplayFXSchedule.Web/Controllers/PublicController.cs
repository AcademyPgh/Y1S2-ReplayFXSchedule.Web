using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    public class PublicController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Public
        public ActionResult Index()
        {
            var result = JsonConvert.SerializeObject(db.ReplayEvents.OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });

            return Content(result, "application/json");
        }

        public ActionResult Categories()
        {
            var result = JsonConvert.SerializeObject(db.ReplayEventTypes.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(result, "application/json");
        }
    }
}