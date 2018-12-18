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

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayEvents
        public ActionResult Index(string search)
        {
            // Bad way to do this but here it is
            var currentYear = DateTime.Parse("1/1/2018");
            //  return View(db.ReplayEvents.Where(x => x.Title.StartsWith(search)|| search == null).ToList());
            return View(db.Events.Where(x => x.Date > currentYear).OrderBy(x => new { x.Date, x.StartTime }).ToList());
        }

        public ContentResult Json()
        {
            var result = JsonConvert.SerializeObject(db.Events.ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Content(result, "application/json");
        }

        // GET: ReplayEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event replayEvent = db.Events.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        // GET: ReplayEvents/Create
        public ActionResult Create()
        {
            ViewBag.EventTypeIDs = "";
            return View();
        }

        // POST: ReplayEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image")] Event replayEvent, string categories, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                replayEvent.Image = azure.GetFileName(upload);
                db.Events.Add(replayEvent);
                replayEvent.EventTypes = new List<EventType>();
                foreach(var id in categories.Split(','))
                {
                    replayEvent.EventTypes.Add(db.EventTypes.Find(Convert.ToInt32(id)));
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayEvent);
        }

        // GET: ReplayEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event replayEvent = db.Events.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventTypeIDs = string.Join(",", replayEvent.EventTypes.Select(r => r.Id));
            return View(replayEvent);
        }

        private string AddType(int id, int typeId)
        {
            Event rpe = db.Events.Find(id);
            EventType rpet = db.EventTypes.Find(typeId);

            rpe.EventTypes.Add(rpet);
            db.SaveChanges();

            return "success";
        }

        private string RemoveType(int id, int typeId)
        {
            Event rpe = db.Events.Find(id);
            EventType typetoremove = new EventType();
            foreach (var item in rpe.EventTypes)
            {
                if (item.Id == typeId)
                {
                    typetoremove = item;
                }
            }
            rpe.EventTypes.Remove(typetoremove);
            db.SaveChanges();
            return "success";
        }

        public ActionResult GetTypes(int id)
        {
            Event rpe = db.Events.Find(id);
            List<EventTypeView> eventList = EventView(rpe.EventTypes.ToList());
            

            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // POST: ReplayEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image")] Event replayEvent, string categories, HttpPostedFileBase upload, string image)
        {
            //int indexExt = 0;
            //string ext = "";
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        azure.deletefromAzure(image);
                        image = null;
                    }
                    replayEvent.Image = azure.GetFileName(upload);
                }
                //Modified entity state causes us to not be able to update connected replayeeventtypes
                Event rpe = db.Events.Find(replayEvent.Id);

                rpe.Title = replayEvent.Title;
                rpe.Date = replayEvent.Date;
                rpe.StartTime = replayEvent.StartTime;
                rpe.EndTime = replayEvent.EndTime;
                rpe.Description = replayEvent.Description;
                rpe.ExtendedDescription = replayEvent.ExtendedDescription;
                rpe.Location = replayEvent.Location;
                rpe.Image = replayEvent.Image;

                SaveReplayEventTypes(replayEvent.Id, categories.Split(','));
                db.SaveChanges();

                //RemoveAllEventTypes(replayEvent.Id);
                //SaveReplayEventTypes(replayEvent.Id, categories.Split(','));
                return RedirectToAction("Index");
            }
            return View(replayEvent);
        }
         private void SaveReplayEventTypes(int id, string[] EventTypeIDs)
        {
            List<int> ids = new List<int>();
            List<EventType> typesToRemove = new List<EventType>();
            foreach (var eventId in EventTypeIDs)
            {
                int i;
                if (int.TryParse(eventId, out i))
                {
                    ids.Add(i);
                }
            }

            var replayEvent = db.Events.Find(id);
            foreach (var eventType in replayEvent.EventTypes)
            {
                if (ids.Contains(eventType.Id))
                {
                    // keep it, remove from the ids list
                    ids.Remove(eventType.Id);
                }
                else
                {
                    
                    typesToRemove.Add(eventType);
                }
            }
            foreach (var type in typesToRemove)
            {
                replayEvent.EventTypes.Remove(type);
            }
            foreach (var i in ids)
            {
                replayEvent.EventTypes.Add(db.EventTypes.Find(i));
            }
        }

        // GET: ReplayEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event replayEvent = db.Events.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        // POST: ReplayEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event replayEvent = db.Events.Find(id);
            if(replayEvent.Image != null)
            {
                azure.deletefromAzure(replayEvent.Image);
            }
            db.Events.Remove(replayEvent);
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

        public ActionResult GetAllEventTypes()
        {
            return Json(EventView(db.EventTypes.ToList()), JsonRequestBehavior.AllowGet);
        }

        private List<EventTypeView> EventView(List<EventType> baseList)
        {
            List<EventTypeView> outList = new List<EventTypeView>();

            foreach (var item in baseList)
            {
                EventTypeView temp = new EventTypeView
                {
                    Name = item.Name,
                    Id = item.Id,
                    DisplayName = item.DisplayName
                };
                outList.Add(temp);

            }
            return outList;
        }
    }
}
