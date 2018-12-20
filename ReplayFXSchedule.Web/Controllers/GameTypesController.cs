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
    public class GameTypesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayGameTypes
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
            return View(convention.GameTypes.ToList());
        }

        // GET: ReplayGameTypes/Details/5
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
            GameType replayGameType = db.GameTypes.Find(id);
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Create
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

        // POST: ReplayGameTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, HeaderImage")] GameType replayGameType, int convention_id, HttpPostedFileBase headerImageFile)
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
                if (headerImageFile != null)
                {
                    replayGameType.HeaderImage = azure.GetFileName(headerImageFile);
                }
                convention.GameTypes.Add(replayGameType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Edit/5
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
            GameType replayGameType = convention.GameTypes.Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit([Bind(Include = "Id,Name")] GameType replayGameType, int convention_id, HttpPostedFileBase headerImageFile)
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
                var gt = db.GameTypes.Where(g => g.Id == replayGameType.Id).FirstOrDefault();
                var deleted = false;
                if (gt.HeaderImage != replayGameType.HeaderImage)
                {
                    if (!string.IsNullOrEmpty(gt.HeaderImage))
                    {
                        azure.deletefromAzure(gt.HeaderImage);
                        gt.HeaderImage = null;
                        deleted = true;
                    }
                }
                if (headerImageFile != null)
                {
                    if (!deleted & !string.IsNullOrEmpty(gt.HeaderImage))
                    {
                        azure.deletefromAzure(gt.HeaderImage);
                        gt.HeaderImage = null;
                        deleted = true;
                    }
                    replayGameType.HeaderImage = azure.GetFileName(headerImageFile);
                }

                gt.Name = replayGameType.Name;
                gt.HeaderImage = replayGameType.HeaderImage;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGameType);
        }

        // GET: ReplayGameTypes/Delete/5
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
            GameType replayGameType = convention.GameTypes.Where(c => c.Id == id).FirstOrDefault();
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }

        // POST: ReplayGameTypes/Delete/5
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
            GameType replayGameType = convention.GameTypes.Where(c => c.Id == id).FirstOrDefault();
            db.GameTypes.Remove(replayGameType);
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
