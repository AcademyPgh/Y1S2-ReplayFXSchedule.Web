using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class ReplayVendorsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayVendors
        public ActionResult Index()
        {
            return View(db.ReplayVendors.ToList());
        }

        // GET: ReplayVendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayVendor replayVendor = db.ReplayVendors.Find(id);
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // GET: ReplayVendors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayVendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ExtendedDescription,Location,Image,Url")] ReplayVendor replayVendor, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                replayVendor.Image = azure.GetFileName(upload);
                db.ReplayVendors.Add(replayVendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayVendor);
        }

        // GET: ReplayVendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayVendor replayVendor = db.ReplayVendors.Find(id);
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // POST: ReplayVendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ExtendedDescription,Location,Image,Url")] ReplayVendor replayVendor, HttpPostedFileBase upload, string image)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        azure.deletefromAzure(image);
                        image = null;
                    }
                    replayVendor.Image = azure.GetFileName(upload);
                }
                db.Entry(replayVendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayVendor);
        }

        // GET: ReplayVendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayVendor replayVendor = db.ReplayVendors.Find(id);
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // POST: ReplayVendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReplayVendor replayVendor = db.ReplayVendors.Find(id);
            if (replayVendor.Image != null)
            { azure.deletefromAzure(replayVendor.Image); }
            db.ReplayVendors.Remove(replayVendor);
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
