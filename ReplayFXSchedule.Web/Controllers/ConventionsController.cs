using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class ConventionsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }

        // GET: Conventions
        public ActionResult Index()
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            var conventions = db.AppUserPermissions.Where(aup => aup.AppUser.Id == user.Id && aup.UserRole == UserRole.Admin).Select(x => x.Convention).ToList();
            ViewBag.isAdmin = user.isSuperAdmin;
            return View(conventions);
        }

        // GET: Conventions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // GET: Conventions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartDate,EndDate,Name,Address,Address2,City,State,Zip,HeaderImage,Hashtag,MapImage,EnableInApp,TicketUrl,Url")] Convention convention)
        {
            if (ModelState.IsValid)
            {
                db.Conventions.Add(convention);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(convention);
        }

        // GET: Conventions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // POST: Conventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate,Name,Address,Address2,City,State,Zip,HeaderImage,Hashtag,MapImage,EnableInApp,TicketUrl,Url")] Convention convention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(convention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(convention);
        }

        // GET: Conventions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // POST: Conventions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Convention convention = db.Conventions.Find(id);
            db.Conventions.Remove(convention);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
