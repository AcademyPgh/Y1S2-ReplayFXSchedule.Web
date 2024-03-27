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
            return View(db.Conventions.Find(convention_id));
        }

        // GET: Screens/:convention_id/Location/:id
        public ActionResult Location(int convention_id, int id)
        {
            var con = db.Conventions.Find(convention_id);
            var location = con.GameLocations.Where(gl => gl.Id == id).FirstOrDefault();
            if(location == null)
            {
                return HttpNotFound();
            }

            ViewBag.location = location;
            return View("Index", con);
        }

        public ActionResult List(int convention_id)
        {
            var con = db.Conventions.Find(convention_id);
            return View(con);
        }
    }
}