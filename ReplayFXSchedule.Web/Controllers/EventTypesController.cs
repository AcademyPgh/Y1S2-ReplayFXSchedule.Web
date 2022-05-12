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
    public class EventTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayEventTypes
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

            return View(convention.EventTypes.ToList());
        }

        // GET: ReplayEventTypes/Details/5
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
            EventType replayEventType = convention.EventTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayEventType == null)
            {
                return HttpNotFound();
            }
            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Create
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

            var eventMenus = new List<SelectListItem>();
            eventMenus.Add(new SelectListItem
            {
                Text = "None",
                Value = "0",
                Selected = true
            });
            eventMenus.AddRange(convention.EventMenus
                .Select(em =>
                    new SelectListItem
                    {
                        Text = em.DisplayName,
                        Value = em.Id.ToString(),
                        Selected = false
                    })
                .ToList());
            ViewBag.EventMenus = eventMenus;
            return View();
        }

        // POST: ReplayEventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] EventType replayEventType, int convention_id, int EventMenus)
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
                replayEventType.EventMenu = convention.EventMenus.Where(em => em.Id == EventMenus).FirstOrDefault();
                convention.EventTypes.Add(replayEventType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Edit/5
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
            EventType replayEventType = convention.EventTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayEventType == null)
            {
                return HttpNotFound();
            }

            var eventMenus = new List<SelectListItem>();
            eventMenus.Add(new SelectListItem
            {
                Text = "None",
                Value = "0",
                Selected = false
            });
            eventMenus.AddRange(convention.EventMenus
                .Select(em =>
                    new SelectListItem
                    {
                        Text = em.DisplayName,
                        Value = em.Id.ToString(),
                        Selected = replayEventType.EventMenu != null && em.Id == replayEventType.EventMenu.Id
                    })
                .ToList());
            ViewBag.EventMenus = eventMenus;

            return View(replayEventType);
        }

        // POST: ReplayEventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] EventType replayEventType, int convention_id, int EventMenus)
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
                var rpe = convention.EventTypes.Where(et => et.Id == replayEventType.Id).FirstOrDefault();
                rpe.DisplayName = replayEventType.DisplayName;
                rpe.IsMenu = replayEventType.IsMenu;
                rpe.IsPrivate = replayEventType.IsPrivate;
                rpe.EventMenu = convention.EventMenus.Where(em => em.Id == EventMenus).FirstOrDefault();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Delete/5
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
            EventType replayEventType = convention.EventTypes.Where(e => e.Id == id).FirstOrDefault();
            if (replayEventType == null)
            {
                return HttpNotFound();
            }
            return View(replayEventType);
        }

        // POST: ReplayEventTypes/Delete/5
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

            EventType replayEventType = convention.EventTypes.Where(e => e.Id == id).FirstOrDefault();
            db.EventTypes.Remove(replayEventType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult BulkLoad(int convention_id)
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

            return View(convention.EventTypes.ToList());
        }
        [HttpPost]
        public ActionResult BulkLoad(int convention_id, string EventTypes)
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

            var types = EventTypes.Split();

            var ets = types.Where(t => t.Length > 0).Select(t => new EventType { Name = t.ToLower(), DisplayName = t }).ToList();
            convention.EventTypes.AddRange(ets);
            db.SaveChanges();

            return View(convention.EventTypes.ToList());
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
