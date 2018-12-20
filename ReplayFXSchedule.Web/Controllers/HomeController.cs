using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Home
        public ActionResult Index()
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            var conventions = db.AppUserPermissions.Where(aup => aup.AppUser.Id == user.Id && aup.UserRole == UserRole.Admin).Select(x => x.Convention).ToList();
            return View(conventions);
        }

        public ActionResult ConventionHome(int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return View("NoConferencePermissions");
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            return View(convention);
        }

        public ActionResult NoConventionPermissions()
        {
            return View();
        }
    }
}