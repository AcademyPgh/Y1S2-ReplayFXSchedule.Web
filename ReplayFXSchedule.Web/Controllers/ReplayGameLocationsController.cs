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
    public class ReplayGameLocationsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGameLocations
        public ActionResult Index()
        {
            return View(db.ReplayGameLocations.ToList());
        }

        // GET: ReplayGameLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameLocation replayGameLocation = db.ReplayGameLocations.Find(id);
            if (replayGameLocation == null)
            {
                return HttpNotFound();
            }
            return View(replayGameLocation);
        }

        // GET: ReplayGameLocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayGameLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Location")] ReplayGameLocation replayGameLocation)
        {
            if (ModelState.IsValid)
            {
                db.ReplayGameLocations.Add(replayGameLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGameLocation);
        }

        // GET: ReplayGameLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameLocation replayGameLocation = db.ReplayGameLocations.Find(id);
            if (replayGameLocation == null)
            {
                return HttpNotFound();
            }
            return View(replayGameLocation);
        }

        // POST: ReplayGameLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Location")] ReplayGameLocation replayGameLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayGameLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGameLocation);
        }

        // GET: ReplayGameLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameLocation replayGameLocation = db.ReplayGameLocations.Find(id);
            if (replayGameLocation == null)
            {
                return HttpNotFound();
            }
            return View(replayGameLocation);
        }

        // POST: ReplayGameLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReplayGameLocation replayGameLocation = db.ReplayGameLocations.Find(id);
            db.ReplayGameLocations.Remove(replayGameLocation);
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
