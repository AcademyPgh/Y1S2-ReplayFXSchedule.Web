using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class ReplayGameLocationController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGameLocation
        public ActionResult Index()
        {
            return View(db.ReplayGameLocations.ToList());
        }

        // GET: ReplayGameLocation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult
                    (HttpStatusCode.BadRequest);
            }
            ReplayGameLocation replayGameLocation = db.ReplayGameLocations.Find(id);
            if (replayGameLocation == null)
            {
                return HttpNotFound();
            }
            return View(replayGameLocation);
        }


        // GET: ReplayGameLocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayGameLocation/Create
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

        // GET: ReplayGameLocation/Edit/5
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

        // POST: ReplayGameLocation/Edit/5
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

        // GET: ReplayGameLocation/Delete/5
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

        // POST: ReplayEventTypes/Delete/5
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