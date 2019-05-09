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
    public class DisplayMessagesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: DisplayMessages
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
            return View(convention.DisplayMessages.ToList());
        }

        // GET: DisplayMessages/Details/5
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
            DisplayMessage displayMessages = convention.DisplayMessages.Where(dm => dm.Id == id).FirstOrDefault();
            if (displayMessages == null)
            {
                return HttpNotFound();
            }
            return View(displayMessages);
        }

        // GET: DisplayMessages/Create
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

        // POST: DisplayMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,StartTime,EndTime,Title,Text,Image")] DisplayMessage displayMessages, int convention_id, HttpPostedFileBase imageFile)
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
                if (imageFile != null)
                {
                    displayMessages.Image = azure.GetFileName(imageFile);
                }
                convention.DisplayMessages.Add(displayMessages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayMessages);
        }

        // GET: DisplayMessages/Edit/5
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
            DisplayMessage displayMessages = convention.DisplayMessages.Where(dm => dm.Id == id).FirstOrDefault();
            if (displayMessages == null)
            {
                return HttpNotFound();
            }
            return View(displayMessages);
        }

        // POST: DisplayMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,StartTime,EndTime,Title,Text,Image")] DisplayMessage displayMessages, int convention_id, HttpPostedFileBase imageFile)
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
                var dm = convention.DisplayMessages.Where(d => d.Id == displayMessages.Id).FirstOrDefault();
                dm.Date = displayMessages.Date;
                dm.StartTime = displayMessages.StartTime;
                dm.EndTime = displayMessages.EndTime;
                dm.Title = displayMessages.Title;
                dm.Text = displayMessages.Text;

                var deleted = false;
                if (dm.Image != displayMessages.Image)
                {
                    if (!string.IsNullOrEmpty(dm.Image))
                    {
                        azure.deletefromAzure(dm.Image);
                        dm.Image = null;
                        deleted = true;
                    }
                }
                if (imageFile != null)
                {
                    if (!deleted & !string.IsNullOrEmpty(dm.Image))
                    {
                        azure.deletefromAzure(dm.Image);
                        dm.Image = null;
                        deleted = true;
                    }
                    dm.Image = azure.GetFileName(imageFile);
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayMessages);
        }

        // GET: DisplayMessages/Delete/5
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
            DisplayMessage displayMessages = db.DisplayMessages.Find(id);
            if (displayMessages == null)
            {
                return HttpNotFound();
            }
            return View(displayMessages);
        }

        // POST: DisplayMessages/Delete/5
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
            DisplayMessage displayMessages = convention.DisplayMessages.Where(dm => dm.Id == id).FirstOrDefault();
            if(displayMessages == null)
            {
                return new HttpNotFoundResult();
            }
            if (!string.IsNullOrEmpty(displayMessages.Image))
            {
                azure.deletefromAzure(displayMessages.Image);
            }
            db.DisplayMessages.Remove(displayMessages);
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
