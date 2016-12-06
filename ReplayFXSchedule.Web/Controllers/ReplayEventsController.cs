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
{
    public class ReplayEventsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayEvents
        public ActionResult Index()
        {
            return View(db.ReplayEvents.ToList());
        }

        public ContentResult Json()
        {
            var result = JsonConvert.SerializeObject(db.ReplayEvents.ToList(), Formatting.None,
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
            ReplayEvent replayEvent = db.ReplayEvents.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        // GET: ReplayEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplayEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location")] ReplayEvent replayEvent, string categories)
        {
            if (ModelState.IsValid)
            {
                db.ReplayEvents.Add(replayEvent);
                replayEvent.ReplayEventTypes = new List<ReplayEventType>();
                foreach(var id in categories.Split(','))
                {
                    replayEvent.ReplayEventTypes.Add(db.ReplayEventTypes.Find(Convert.ToInt32(id)));
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
            ReplayEvent replayEvent = db.ReplayEvents.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        public string AddType(int id, int typeId)
        {
            ReplayEvent rpe = db.ReplayEvents.Find(id);
            ReplayEventType rpet = db.ReplayEventTypes.Find(typeId);

            rpe.ReplayEventTypes.Add(rpet);
            db.SaveChanges();

            return "success";
        }

        public string RemoveType(int id, int typeId)
        {
            ReplayEvent rpe = db.ReplayEvents.Find(id);
            ReplayEventType typetoremove = new ReplayEventType();
            foreach (var item in rpe.ReplayEventTypes)
            {
                if (item.Id == typeId)
                {
                    typetoremove = item;
                }
            }
            rpe.ReplayEventTypes.Remove(typetoremove);
            db.SaveChanges();
            return "success";
        }

        public ActionResult GetTypes(int id)
        {
            ReplayEvent rpe = db.ReplayEvents.Find(id);
            List<ReplayEventTypeView> eventList = ReplayEventView(rpe.ReplayEventTypes.ToList());
            

            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // POST: ReplayEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location")] ReplayEvent replayEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replayEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayEvent);
        }

        // GET: ReplayEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplayEvent replayEvent = db.ReplayEvents.Find(id);
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
            ReplayEvent replayEvent = db.ReplayEvents.Find(id);
            db.ReplayEvents.Remove(replayEvent);
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
            return Json(ReplayEventView(db.ReplayEventTypes.ToList()), JsonRequestBehavior.AllowGet);
        }

        private List<ReplayEventTypeView> ReplayEventView(List<ReplayEventType> baseList)
        {
            List<ReplayEventTypeView> outList = new List<ReplayEventTypeView>();

            foreach (var item in baseList)
            {
                ReplayEventTypeView temp = new ReplayEventTypeView
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
