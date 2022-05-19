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
    public class ScreenImagesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ScreenImages
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
            //ScreenImage image = convention.ScreenImages.Where(e => e.Id == id).FirstOrDefault();
            //if (image == null)
            //{
            //    return HttpNotFound();
            //}

            return View(convention.ScreenImages.ToList());
        }

        // GET: ScreenImages/Details/5
        public ActionResult Details(int? id, int convention_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

            ScreenImage screenImage = convention.ScreenImages.Where(e => e.Id == id).FirstOrDefault();
            if (screenImage == null)
            {
                return HttpNotFound();
            }
            return View(screenImage);
        }

        // GET: ScreenImages/Create
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

        // POST: ScreenImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Image")] ScreenImage screenImage, int convention_id, HttpPostedFileBase upload)
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
                    AzureTools azure = new AzureTools();
                    screenImage.Name = azure.GetFileName(upload);
                }
                convention.ScreenImages.Add(screenImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(screenImage);
        }

        // GET: ScreenImages/Edit/5
        public ActionResult Edit(int? id, int convention_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

            ScreenImage screenImage = convention.ScreenImages.Where(e => e.Id == id).FirstOrDefault();
            if (screenImage == null)
            {
                return HttpNotFound();
            }
            return View(screenImage);
        }

        // POST: ScreenImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image")] ScreenImage screenImage, int convention_id, HttpPostedFileBase upload)
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
                if(upload != null)
                {
                    AzureTools azure = new AzureTools();
                    azure.deletefromAzure(screenImage.Name);
                    screenImage.Name = azure.GetFileName(upload);
                }

                db.Entry(screenImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(screenImage);
        }

        // GET: ScreenImages/Delete/5
        public ActionResult Delete(int? id, int convention_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

            ScreenImage screenImage = convention.ScreenImages.Where(e => e.Id == id).FirstOrDefault();
            if (screenImage == null)
            {
                return HttpNotFound();
            }
            return View(screenImage);
        }

        // POST: ScreenImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int convention_id)
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

            ScreenImage screenImage = convention.ScreenImages.Where(e => e.Id == id).FirstOrDefault();
            if (screenImage == null)
            {
                return HttpNotFound();
            }
            db.ScreenImages.Remove(screenImage);
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
