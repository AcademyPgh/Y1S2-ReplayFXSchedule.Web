using ReplayFXSchedule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    public class ScreensController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Screens/:convention_id
        public ActionResult Index(int convention_id)
        {
            ViewBag.convention_id = convention_id;
            return View();
        }

        // GET: Screens/:convention_id/Location/:id
        public ActionResult Location(int convention_id, int id)
        {
            ViewBag.convention_id = convention_id;
            ViewBag.location_name = id;

            return View("Index", db.GameLocations.Find(id));
        }
    }
}