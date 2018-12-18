 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class EventTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayEventTypes
        public ActionResult Index()
        {
            return View(db.EventTypes.ToList());
        }

        // GET: ReplayEventTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType replayEventType = db.EventTypes.Find(id);
            if (replayEventType == null)
            {
                return HttpNotFound();
            }
            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayEventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DisplayName")] EventType replayEventType)
        {
            if (ModelState.IsValid)
            {
                db.EventTypes.Add(replayEventType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType replayEventType = db.EventTypes.Find(id);
            if (replayEventType == null)
            {
                return HttpNotFound();
            }
            return View(replayEventType);
        }

        // POST: ReplayEventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayName")] EventType replayEventType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayEventType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayEventType);
        }

        // GET: ReplayEventTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType replayEventType = db.EventTypes.Find(id);
            if (replayEventType == null)
            {
                return HttpNotFound();
            }
            return View(replayEventType);
        }

        // POST: ReplayEventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventType replayEventType = db.EventTypes.Find(id);
            db.EventTypes.Remove(replayEventType);
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
