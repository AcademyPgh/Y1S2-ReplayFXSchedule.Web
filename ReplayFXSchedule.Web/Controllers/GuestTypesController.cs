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
    public class GuestTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: GuestTypes
        public ActionResult Index()
        {
            return View(db.GuestTypes.ToList());
        }

        // GET: GuestTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType guestType = db.GuestTypes.Find(id);
            if (guestType == null)
            {
                return HttpNotFound();
            }
            return View(guestType);
        }

        // GET: GuestTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] GuestType guestType)
        {
            if (ModelState.IsValid)
            {
                db.GuestTypes.Add(guestType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guestType);
        }

        // GET: GuestTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType guestType = db.GuestTypes.Find(id);
            if (guestType == null)
            {
                return HttpNotFound();
            }
            return View(guestType);
        }

        // POST: GuestTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayName,IsPrivate,IsMenu")] GuestType guestType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guestType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guestType);
        }

        // GET: GuestTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestType guestType = db.GuestTypes.Find(id);
            if (guestType == null)
            {
                return HttpNotFound();
            }
            return View(guestType);
        }

        // POST: GuestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuestType guestType = db.GuestTypes.Find(id);
            db.GuestTypes.Remove(guestType);
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
