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
    public class EventMenusController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: EventMenus
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
            return View(convention.EventMenus.ToList());
        }

        // GET: EventMenus/Details/5
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
            EventMenu eventMenu = convention.EventMenus.Where(e => e.Id == id).FirstOrDefault();
            if (eventMenu == null)
            {
                return HttpNotFound();
            }
            return View(eventMenu);
        }

        // GET: EventMenus/Create
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

        // POST: EventMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int convention_id, [Bind(Include = "Id,Display,Name")] EventMenu eventMenu)
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
                convention.EventMenus.Add(eventMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventMenu);
        }

        // GET: EventMenus/Edit/5
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
            EventMenu eventMenu = convention.EventMenus.Where(e => e.Id == id).FirstOrDefault();
            if (eventMenu == null)
            {
                return HttpNotFound();
            }
            return View(eventMenu);
        }

        // POST: EventMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int convention_id, [Bind(Include = "Id,Display,Name")] EventMenu eventMenu)
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
                db.Entry(eventMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventMenu);
        }

        // GET: EventMenus/Delete/5
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
            EventMenu eventMenu = convention.EventMenus.Where(e => e.Id == id).FirstOrDefault();
            if (eventMenu == null)
            {
                return HttpNotFound();
            }
            return View(eventMenu);
        }

        // POST: EventMenus/Delete/5
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

            EventMenu eventMenu = convention.EventMenus.Where(e => e.Id == id).FirstOrDefault();
            db.EventMenus.Remove(eventMenu);
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
