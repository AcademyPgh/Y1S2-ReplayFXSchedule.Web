﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;
using Newtonsoft.Json;

namespace ReplayFXSchedule.Web.Controllers
{   [Authorize]
    public class ReplayGamesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayGames
        public ActionResult Index()
        {
            return View(db.ReplayGames.ToList());
        }

        public ContentResult Json()
        {
            var result = JsonConvert.SerializeObject(db.ReplayGames.ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Content(result, "application/json");
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
            ViewBag.ReplayGameLocationIDs = "";
            ViewBag.ReplayGameTypes = db.ReplayGameTypes.ToList();
            return View();
        }


        // POST: ReplayGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GameTitle,Overview,ReleaseDate,Developer,Genre,Players,ReplayGameType.Id")] ReplayGame replayGame, string locations)
        {
            if (ModelState.IsValid)
            {
                db.ReplayGames.Add(replayGame);
                //replayGame.ReplayGameType=(db.ReplayGameTypes.Find(Convert.ToInt32(gametype)));

                replayGame.ReplayGameLocations = new List<ReplayGameLocation>();
                foreach (var id in locations.Split(','))
                {
                    replayGame.ReplayGameLocations.Add(db.ReplayGameLocations.Find
                        (Convert.ToInt32(id)));
                }
              
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
        
           ViewBag.ReplayGameLocationIDs = string.Join(",", replayGame.ReplayGameLocations.Select(r => r.Id));

            return View(replayGame);
            }

    private string AddGameLocation(int id, int gameLocationId)
        {
            ReplayGame rpg = db.ReplayGames.Find(id);
            ReplayGameLocation rpgl = db.ReplayGameLocations.Find(gameLocationId);

            rpg.ReplayGameLocations.Add(rpgl);
            db.SaveChanges();

            return "success";
        }

        private string RemoveGameLocation(int id, int gameLocationId)
        {
            ReplayGame rpg = db.ReplayGames.Find(id);
            ReplayGameLocation locationtoremove = new ReplayGameLocation();
            foreach (var item in rpg.ReplayGameLocations)
            {
                if (item.Id == gameLocationId)
                {
                    locationtoremove = item;
                }
            }
            rpg.ReplayGameLocations.Remove(locationtoremove);
            db.SaveChanges();
            return "success";
        }


        public ActionResult GetGameLocations(int id)
        {
            ReplayGame rpg = db.ReplayGames.Find(id);
            List<ReplayGameLocation> gameList = ReplayGameView(rpg.ReplayGameLocations.ToList());
      
            return Json(gameList, JsonRequestBehavior.AllowGet);
        }
         

        // POST: ReplayGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GameTitle,Overview,ReleaseDate,Developer,Genre,Players,ReplayGameType.Id")] ReplayGame replayGame, string locations)
        {
            if (ModelState.IsValid)
            {
                ReplayGame rpg = db.ReplayGames.Find(replayGame.Id);
                rpg.GameTitle = replayGame.GameTitle;
                rpg.Overview = replayGame.Overview;
                rpg.ReleaseDate = replayGame.ReleaseDate;
                rpg.Developer = replayGame.Developer;
                rpg.Genre = replayGame.Genre;
                rpg.Players = replayGame.Players;
              //  rpg.ReplayGameType = (db.ReplayGameTypes.Find(Convert.ToInt32(gametype)));

                SaveReplayGameLocations(replayGame.Id, locations.Split(','));

                db.SaveChanges();


                return RedirectToAction("Index");
            }
            return View(replayGame);
        }

        private void SaveReplayGameLocations(int id, string[] GameLocationIDs)
        {
            {
                List<int> ids = new List<int>();
                List<ReplayGameLocation> locationsToRemove = new List<ReplayGameLocation>();
                foreach (var gameId in GameLocationIDs)
                {
                    int i;
                    if (int.TryParse(gameId, out i))
                    {
                        ids.Add(i);
                    }
                }

                var replayGame = db.ReplayGames.Find(id);
                foreach (var gameLocation in replayGame.ReplayGameLocations)
                {
                    if (ids.Contains(gameLocation.Id))
                    {
                        // keep it, remove from the ids list
                        ids.Remove(gameLocation.Id);
                    }
                    else
                    {

                        locationsToRemove.Add(gameLocation);
                    }
                }
                foreach (var location in locationsToRemove)
                {
                    replayGame.ReplayGameLocations.Remove(location);
                }
                foreach (var i in ids)
                {
                    replayGame.ReplayGameLocations.Add(db.ReplayGameLocations.Find(i));
                }
            }
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

        public ActionResult GetAllGameLocations()
        {
            return Json(ReplayGameView(db.ReplayGameLocations.ToList()), JsonRequestBehavior.AllowGet);
        }

             private List<ReplayGameLocation> ReplayGameView(List<ReplayGameLocation> baseList)
        {
            List<ReplayGameLocation> exitList = new List<ReplayGameLocation>();

            foreach (var item in baseList)
            {
                ReplayGameLocation temp = new ReplayGameLocation
                {
                    Location = item.Location,
                    Id = item.Id,
                };
                exitList.Add(temp);

            }
            return exitList;
        }


    }
}
