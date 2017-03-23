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
    public class ReplayGameTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGameTypes
        public ActionResult Index()
        {
            return View(db.ReplayGameTypes.ToList());
        }

        // GET: ReplayGameTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameType replayGameType = db.ReplayGameTypes.Find(id);
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayGameTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ReplayGameType replayGameType)
        {
            if (ModelState.IsValid)
            {
                db.ReplayGameTypes.Add(replayGameType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameType replayGameType = db.ReplayGameTypes.Find(id);
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }

        // POST: ReplayGameTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ReplayGameType replayGameType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayGameType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGameType replayGameType = db.ReplayGameTypes.Find(id);
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }

        // POST: ReplayGameTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReplayGameType replayGameType = db.ReplayGameTypes.Find(id);
            db.ReplayGameTypes.Remove(replayGameType);
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
