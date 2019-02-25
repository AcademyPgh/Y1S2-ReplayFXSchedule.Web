using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    public class ScreensController : Controller
    {
        // GET: Screens
        public ActionResult Index(int convention_id)
        {
            ViewBag.convention_id = convention_id;
            return View();
        }
    }
}