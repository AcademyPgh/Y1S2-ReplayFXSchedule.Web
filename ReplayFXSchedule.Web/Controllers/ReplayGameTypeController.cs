﻿using System;
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
    public class ReplayGameTypeController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGameType
        public ActionResult Index()
        {
            return View(db.GameTypes.ToList());
        }

        // GET: ReplayGameType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult
                    (HttpStatusCode.BadRequest);
            }
            GameType replayGameType = db.GameTypes.Find(id);
            if (replayGameType == null)
            {
                return HttpNotFound();
            }
            return View(replayGameType);
        }


        // GET: ReplayGameType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayGameType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GameType replayGameType)
        {
            if (ModelState.IsValid)
            {
                db.GameTypes.Add(replayGameType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGameType);
        }

        // GET: ReplayGameType/Edit/5
        public ActionResult Edit(int? id)
        {
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

        // POST: ReplayGameType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type")] GameType replayGameType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayGameType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGameType);
        }

        // GET: ReplayGameType/Delete/5
        public ActionResult Delete(int? id)
        {
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

        // POST: ReplayEventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameType replayGameType = db.GameTypes.Find(id);
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