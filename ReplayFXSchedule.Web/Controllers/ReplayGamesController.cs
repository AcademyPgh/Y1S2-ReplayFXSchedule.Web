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
    public class ReplayGamesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGames
        public ActionResult Index()
        {
            return View(db.ReplayGames.ToList());
        }

        // GET: ReplayGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGame replayGame = db.ReplayGames.Find(id);
            if (replayGame == null)
            {
                return HttpNotFound();
            }
            return View(replayGame);
        }

        // GET: ReplayGames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GameTitle,Overview,ReleaseDate,Developer,Genres,Players")] ReplayGame replayGame)
        {
            if (ModelState.IsValid)
            {
                db.ReplayGames.Add(replayGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGame);
        }

        // GET: ReplayGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGame replayGame = db.ReplayGames.Find(id);
            if (replayGame == null)
            {
                return HttpNotFound();
            }
            return View(replayGame);
        }

        // POST: ReplayGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GameTitle,Overview,ReleaseDate,Developer,Genres,Players")] ReplayGame replayGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGame);
        }

        // GET: ReplayGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayGame replayGame = db.ReplayGames.Find(id);
            if (replayGame == null)
            {
                return HttpNotFound();
            }
            return View(replayGame);
        }

        // POST: ReplayGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReplayGame replayGame = db.ReplayGames.Find(id);
            db.ReplayGames.Remove(replayGame);
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
