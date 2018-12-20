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
    public class VendorsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayVendors
        public ActionResult Index(int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            return View(convention.Vendors.ToList());
        }

        // GET: ReplayVendors/Details/5
        public ActionResult Details(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // GET: ReplayVendors/Create
        public ActionResult Create(int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            return View();
        }

        // POST: ReplayVendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ExtendedDescription,Location,Image,Url")] Vendor replayVendor, HttpPostedFileBase upload, int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            if (ModelState.IsValid)
            {
                replayVendor.Image = azure.GetFileName(upload);
                convention.Vendors.Add(replayVendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayVendor);
        }

        // GET: ReplayVendors/Edit/5
        public ActionResult Edit(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ExtendedDescription,Location,Image,Url")] Vendor replayVendor, HttpPostedFileBase upload, string image, int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
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
        public ActionResult Delete(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // POST: ReplayVendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int convention_id, int id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                return new HttpNotFoundResult();
            }
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor.Image != null)
            { azure.deletefromAzure(replayVendor.Image); }
            db.Vendors.Remove(replayVendor);
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
