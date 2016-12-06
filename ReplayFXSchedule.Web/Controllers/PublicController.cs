using Newtonsoft.Json;
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
            var result = JsonConvert.SerializeObject(db.ReplayEvents.ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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