using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;
using Newtonsoft.Json;
using ReplayFXSchedule.Web.Shared;
using System.Security.Claims;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayGames

        //the first parameter is the option that we choose and the second parameter will use the textbox value   
        public ActionResult Index(string search)
        {
            var user_service = new UserService((ClaimsIdentity)User.Identity, db);
            //if a user chooses the radio button option as Game Title   
            //Index action method will return a view with games based on what a user specifies the value is in the textbox   
            return View(db.Games.Where(x => x.GameTitle.Contains(search)|| search == null).ToList());
        }
       
        public ContentResult Json()
        {
            var result = JsonConvert.SerializeObject(db.Games.ToList(), Formatting.None,
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
            Game replayGame = db.Games.Find(id);
            if (replayGame == null)
            {
                return HttpNotFound();
            }
            return View(replayGame);
        }

        // GET: ReplayGames/Create
        public ActionResult Create()
        {
            ViewBag.GameLocationIDs = "";
            ViewBag.GameTypes = db.GameTypes.ToList();
            return View();
        }


        // POST: ReplayGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GameTitle,Overview,AtReplay,ReleaseDate,Developer,Genre,Players,Image")] Game replayGame, string gametype, string locations, HttpPostedFileBase upload)
         {
            replayGame.GameType = (db.GameTypes.Find(Convert.ToInt32(gametype)));
            ModelState.Clear();
            TryValidateModel(replayGame);
            if (ModelState.IsValid)
            {
                replayGame.Image = azure.GetFileName(upload);
                db.Games.Add(replayGame);


                if (locations != "")
                {
                    replayGame.GameLocations = new List<GameLocation>();
                    foreach (var id in locations.Split(','))
                    {
                        replayGame.GameLocations.Add(db.GameLocations.Find
                            (Convert.ToInt32(id)));
                    }
                }
              
            db.SaveChanges();
            return RedirectToAction("Index");
        }
            ViewBag.GameTypes = db.GameTypes.ToList();
            return View(replayGame);
        }

        // GET: ReplayGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game replayGame = db.Games.Find(id);
            if (replayGame == null)
            {
                return HttpNotFound();
            }
        
            ViewBag.GameLocationIDs = string.Join(",", replayGame.GameLocations.Select(r => r.Id));
            ViewBag.GameTypes = db.GameTypes.ToList();

            return View(replayGame);
        }

        private string AddGameLocation(int id, int gameLocationId)
        {
            Game rpg = db.Games.Find(id);
            GameLocation rpgl = db.GameLocations.Find(gameLocationId);

            rpg.GameLocations.Add(rpgl);
            db.SaveChanges();

            return "success";
        }

        private string RemoveGameLocation(int id, int gameLocationId)
        {
            Game rpg = db.Games.Find(id);
            GameLocation locationtoremove = new GameLocation();
            foreach (var item in rpg.GameLocations)
            {
                if (item.Id == gameLocationId)
                {
                    locationtoremove = item;
                }
            }
            rpg.GameLocations.Remove(locationtoremove);
            db.SaveChanges();
            return "success";
        }


        public ActionResult GetGameLocations(int id)
        {
            Game rpg = db.Games.Find(id);
            List<GameLocation> gameList = ReplayGameView(rpg.GameLocations.ToList());
      
            return Json(gameList, JsonRequestBehavior.AllowGet);
        }
         

        // POST: ReplayGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GameTitle,Overview,AtReplay,ReleaseDate,Developer,Genre,Players,Image,ReplayGameType.Id")] Game replayGame, string gametype, string locations, HttpPostedFileBase upload, string image)
        {
            replayGame.GameType = (db.GameTypes.Find(Convert.ToInt32(gametype)));
            ModelState.Clear();
            TryValidateModel(replayGame);
            if (ModelState.IsValid)
            {
                if (upload != null)
                { 
                    if (!string.IsNullOrEmpty(image))
                    {
                        azure.deletefromAzure(image);
                        image = null;
                    }
                    replayGame.Image = azure.GetFileName(upload);
                }
                Game rpg = db.Games.Find(replayGame.Id);
                rpg.GameTitle = replayGame.GameTitle;
                rpg.Overview = replayGame.Overview;
                rpg.ReleaseDate = replayGame.ReleaseDate;
                rpg.Developer = replayGame.Developer;
                rpg.Genre = replayGame.Genre;
                rpg.Players = replayGame.Players;
                rpg.Image = replayGame.Image;
                rpg.AtConvention = replayGame.AtConvention;
                rpg.GameType = (db.GameTypes.Find(Convert.ToInt32(gametype)));

                SaveReplayGameLocations(replayGame.Id, locations.Split(','));

                db.SaveChanges();


                return RedirectToAction("Index");
            }
            //ViewBag.ReplayGameLocationIDs = string.Join(",", replayGame.ReplayGameLocations.Select(r => r.Id));
            ViewBag.GameTypes = db.GameTypes.ToList();
            return View(replayGame);
        }

        public ActionResult SwapAtConventionValue(int id)
        {
            var game = db.Games.Where(g => g.Id == id).Include(g => g.GameType).FirstOrDefault(); ;
            game.AtConvention = !game.AtConvention;
            db.SaveChanges();
            var result = JsonConvert.SerializeObject(game, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }

        private void SaveReplayGameLocations(int id, string[] GameLocationIDs)
        {
            {
                List<int> ids = new List<int>();
                List<GameLocation> locationsToRemove = new List<GameLocation>();
                foreach (var gameId in GameLocationIDs)
                {
                    int i;
                    if (int.TryParse(gameId, out i))
                    {
                        ids.Add(i);
                    }
                }

                var replayGame = db.Games.Find(id);
                foreach (var gameLocation in replayGame.GameLocations)
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
                    replayGame.GameLocations.Remove(location);
                }
                foreach (var i in ids)
                {
                    replayGame.GameLocations.Add(db.GameLocations.Find(i));
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
            Game replayGame = db.Games.Find(id);
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
            Game replayGame = db.Games.Find(id);
            if (replayGame.Image != null)
            { azure.deletefromAzure(replayGame.Image); }
            db.Games.Remove(replayGame);
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
            return Json(ReplayGameView(db.GameLocations.ToList()), JsonRequestBehavior.AllowGet);
        }

             private List<GameLocation> ReplayGameView(List<GameLocation> baseList)
        {
            List<GameLocation> exitList = new List<GameLocation>();

            foreach (var item in baseList)
            {
                GameLocation temp = new GameLocation
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
