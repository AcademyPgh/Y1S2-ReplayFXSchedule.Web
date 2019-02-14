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
using ReplayFXSchedule.Web.Shared;
using System.Security.Claims;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayEvents
        public ActionResult Index(int convention_id, string search)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if(!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            var convention = db.Conventions.Find(convention_id);
            if(convention == null)
            {
                return new HttpNotFoundResult();
            }
            //  return View(db.ReplayEvents.Where(x => x.Title.StartsWith(search)|| search == null).ToList());
            return View(convention.Events.OrderBy(x => x.Date).ThenBy(x => x.StartTime).ToList());
        }

        public ContentResult Json(int convention_id)
        {
            var result = JsonConvert.SerializeObject(db.Conventions.Find(convention_id).Events.ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Content(result, "application/json");
        }

        // GET: Events/1/Details/5
        public ActionResult Details(int convention_id, int? id)
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

            Event replayEvent = db.Events.Find(id);
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        // GET: Events/1/Create
        public ActionResult Create(int convention_id, int? id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpNotFoundResult();
            }

            Event eventToCopy = null;
            if (id != null)
            {
                eventToCopy = db.Events.Find(id);
            }
            ViewBag.EventTypeIDs = "";
            ViewBag.ConId = convention_id;
            return View(eventToCopy);
        }

        // POST: Events/1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image,IsPromo,PromoImage")] Event replayEvent, string categories, HttpPostedFileBase upload, HttpPostedFileBase promoUpload, int convention_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            if (!us.IsConventionAdmin(convention_id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var con = db.Conventions.Find(convention_id);
            if(con == null)
            {
                return View(replayEvent);
            }

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    replayEvent.Image = azure.GetFileName(upload);
                }
                if (promoUpload != null)
                {
                    replayEvent.PromoImage = azure.GetFileName(promoUpload);
                }
                con.Events.Add(replayEvent);
                replayEvent.EventTypes = new List<EventType>();
                foreach(var id in categories.Split(','))
                {
                    if (int.TryParse(id, out int Id))
                    {
                        replayEvent.EventTypes.Add(db.EventTypes.Find(Id));
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayEvent);
        }

        // GET: ReplayEvents/Edit/5
        public ActionResult Edit(int convention_id, int? id)
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
            Event replayEvent = convention.Events.Where(e => e.Id == id).FirstOrDefault();
            if (replayEvent == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConId = convention_id;
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

        public ActionResult GetTypes(int convention_id, int id)
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

            Event rpe = convention.Events.Where(e => e.Id == id).FirstOrDefault();
            List<EventTypeView> eventList = EventView(rpe.EventTypes.ToList());
            

            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // POST: ReplayEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image,IsPromo,PromoImage")] Event replayEvent, string categories, HttpPostedFileBase upload, HttpPostedFileBase promoUpload, string image, int convention_id)
        {
            //int indexExt = 0;
            //string ext = "";
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
                Event rpe = convention.Events.Where(e => e.Id == replayEvent.Id).FirstOrDefault();

                var deleteImage = false;
                if (rpe.Image != replayEvent.Image || upload != null)
                {
                    if (!string.IsNullOrEmpty(replayEvent.Image))
                    {
                        if(db.Events.Where(e => e.Convention.Id == convention_id && e.Image == replayEvent.Image).ToList().Count == 1)
                        {
                            // if we are on the last one
                            deleteImage = true;
                        }
                    }
                }
                if (deleteImage)
                {
                    if (!string.IsNullOrEmpty(replayEvent.Image))
                    {
                        azure.deletefromAzure(replayEvent.Image);
                        replayEvent.Image = null;
                    }
                }
                if (upload != null)
                {
                    replayEvent.Image = azure.GetFileName(upload);
                }

                deleteImage = false;
                if (rpe.PromoImage != replayEvent.PromoImage || promoUpload != null)
                {
                    if (!string.IsNullOrEmpty(replayEvent.PromoImage))
                    {
                        if (db.Events.Where(e => e.Convention.Id == convention_id && e.PromoImage == replayEvent.PromoImage).ToList().Count == 1)
                        {
                            // if we are on the last one
                            deleteImage = true;
                        }
                    }
                }
                if (deleteImage)
                {
                    if (!string.IsNullOrEmpty(replayEvent.PromoImage))
                    {
                        azure.deletefromAzure(replayEvent.PromoImage);
                        replayEvent.PromoImage = null;
                    }
                }
                if (promoUpload != null)
                {
                    replayEvent.PromoImage = azure.GetFileName(promoUpload);
                }

                //Modified entity state causes us to not be able to update connected replayeeventtypes

                rpe.Title = replayEvent.Title;
                rpe.Date = replayEvent.Date;
                rpe.StartTime = replayEvent.StartTime;
                rpe.EndTime = replayEvent.EndTime;
                rpe.Description = replayEvent.Description;
                rpe.ExtendedDescription = replayEvent.ExtendedDescription;
                rpe.Location = replayEvent.Location;
                rpe.Image = replayEvent.Image;
                rpe.PromoImage = replayEvent.PromoImage;
                rpe.IsPromo = replayEvent.IsPromo;

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

        // GET: ReplayEvents/1/Delete/5
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
            Event replayEvent = convention.Events.Where(e => e.Id == id).FirstOrDefault();
            if (replayEvent == null)
            {
                return HttpNotFound();
            }
            return View(replayEvent);
        }

        // POST: ReplayEvents/Delete/5
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
            Event replayEvent = convention.Events.Where(e => e.Convention.Id == convention_id && e.Id == id).FirstOrDefault();
            if(replayEvent.Image != null)
            {
                if (db.Events.Where(e => e.Convention.Id == convention_id && e.Image == replayEvent.Image).ToList().Count == 1)
                {
                    // if we are on the last one
                    azure.deletefromAzure(replayEvent.Image);
                }
            }
            if (replayEvent.PromoImage != null)
            {
                if (db.Events.Where(e => e.Convention.Id == convention_id && e.PromoImage == replayEvent.PromoImage).ToList().Count == 1)
                {
                    // if we are on the last one
                    azure.deletefromAzure(replayEvent.PromoImage);
                }
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

        public ActionResult GetAllEventTypes(int convention_id)
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
            return Json(EventView(convention.EventTypes.ToList()), JsonRequestBehavior.AllowGet);
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
