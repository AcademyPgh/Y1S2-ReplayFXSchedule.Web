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
    public class GuestsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: Guests
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
            return View(convention.Guests.OrderBy(v => v.Name).ToList());
        }

        // GET: Guests/Details/5
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
            Guest replayGuest = convention.Guests.Where(v => v.Id == id).FirstOrDefault();
            if (replayGuest == null)
            {
                return HttpNotFound();
            }
            return View(replayGuest);
        }

        // GET: Guests/Create
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

            ViewBag.GuestTypeIDs = "";
            ViewBag.ConId = convention_id;

            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ExtendedDescription,Image,Url")] Guest replayGuest, HttpPostedFileBase upload, int convention_id, string categories)
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
                replayGuest.Image = azure.GetFileName(upload);
                convention.Guests.Add(replayGuest);
                replayGuest.GuestTypes = new List<GuestType>();
                foreach (var id in categories.Split(','))
                {
                    if (int.TryParse(id, out int Id))
                    {
                        replayGuest.GuestTypes.Add(db.GuestTypes.Find(Id));
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayGuest);
        }

        // GET: Guests/Edit/5
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
            Guest replayGuest = convention.Guests.Where(v => v.Id == id).FirstOrDefault();
            if (replayGuest == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConId = convention_id;
            ViewBag.GuestTypeIDs = string.Join(",", replayGuest.GuestTypes.Select(r => r.Id));

            return View(replayGuest);
        }

        private string AddType(int id, int typeId)
        {
            Guest guest = db.Guests.Find(id);
            GuestType guestType = db.GuestTypes.Find(typeId);

            guest.GuestTypes.Add(guestType);
            db.SaveChanges();

            return "success";
        }

        private string RemoveType(int id, int typeId)
        {
            Guest guest = db.Guests.Find(id);
            GuestType typetoremove = new GuestType();
            foreach (var item in guest.GuestTypes)
            {
                if (item.Id == typeId)
                {
                    typetoremove = item;
                }
            }
            guest.GuestTypes.Remove(typetoremove);
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

            Guest rpe = convention.Guests.Where(e => e.Id == id).FirstOrDefault();
            List<GuestTypeView> eventList = GuestTypeView(rpe.GuestTypes.ToList());


            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ExtendedDescription,Image,Url")] Guest replayGuest, HttpPostedFileBase upload, string image, int convention_id, string categories)
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
                var guest = db.Guests.Find(replayGuest.Id);
                if (upload != null)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        azure.deletefromAzure(image);
                        image = null;
                    }
                    replayGuest.Image = azure.GetFileName(upload);
                }

                guest.Name = replayGuest.Name;
                guest.Description = replayGuest.Description;
                guest.ExtendedDescription = replayGuest.ExtendedDescription;
                guest.Image = replayGuest.Image;
                guest.Url = replayGuest.Url;

                SaveGuestTypes(guest.Id, categories.Split(','));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayGuest);
        }
        private void SaveGuestTypes(int id, string[] GuestTypeIDs)
        {
            List<int> ids = new List<int>();
            List<GuestType> typesToRemove = new List<GuestType>();
            foreach (var guestId in GuestTypeIDs)
            {
                int i;
                if (int.TryParse(guestId, out i))
                {
                    ids.Add(i);
                }
            }

            var guest = db.Guests.Find(id);
            foreach (var guestType in guest.GuestTypes)
            {
                if (ids.Contains(guestType.Id))
                {
                    // keep it, remove from the ids list
                    ids.Remove(guestType.Id);
                }
                else
                {

                    typesToRemove.Add(guestType);
                }
            }
            foreach (var type in typesToRemove)
            {
                guest.GuestTypes.Remove(type);
            }
            foreach (var i in ids)
            {
                guest.GuestTypes.Add(db.GuestTypes.Find(i));
            }
        }


        // GET: Guests/Delete/5
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
            Guest replayGuest = convention.Guests.Where(v => v.Id == id).FirstOrDefault();
            if (replayGuest == null)
            {
                return HttpNotFound();
            }
            return View(replayGuest);
        }

        // POST: Guests/Delete/5
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
            Guest replayGuest = convention.Guests.Where(v => v.Id == id).FirstOrDefault();
            if (replayGuest.Image != null)
            { azure.deletefromAzure(replayGuest.Image); }
            db.Guests.Remove(replayGuest);
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

        private List<GuestTypeView> GuestTypeView(List<GuestType> baseList)
        {
            List<GuestTypeView> outList = new List<GuestTypeView>();

            foreach (var item in baseList)
            {
                GuestTypeView temp = new GuestTypeView
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
