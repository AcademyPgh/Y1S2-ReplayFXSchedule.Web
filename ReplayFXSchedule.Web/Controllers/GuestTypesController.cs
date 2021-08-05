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
    public class GuestTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: GuestTypes
        public ActionResult Index(int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }

            return View(convention.GuestTypes.ToList());
        }

        // GET: GuestTypes/Details/5
        public ActionResult Details(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType replayGuestType = convention.GuestTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayGuestType == null)
            {
                return HttpNotFound();
            }
            return View(replayGuestType);
        }

        // GET: GuestTypes/Create
        public ActionResult Create(int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            return View();
        }

        // POST: GuestTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] GuestType replayGuestType, int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }

            if (ModelState.IsValid)
            {
                convention.GuestTypes.Add(replayGuestType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGuestType);
        }

        // GET: GuestTypes/Edit/5
        public ActionResult Edit(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType replayGuestType = convention.GuestTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayGuestType == null)
            {
                return HttpNotFound();
            }
            return View(replayGuestType);
        }

        // POST: GuestTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] GuestType replayGuestType, int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            if (ModelState.IsValid)
            {
                db.Entry(replayGuestType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGuestType);
        }

        // GET: GuestTypes/Delete/5
        public ActionResult Delete(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType replayGuestType = convention.GuestTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayGuestType == null)
            {
                return HttpNotFound();
            }
            return View(replayGuestType);
        }

        // POST: GuestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int convention_id, int id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }

            GuestType replayGuestType = convention.GuestTypes.Where(e => e.Id == id).FirstOrDefault();
            db.GuestTypes.Remove(replayGuestType);
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
