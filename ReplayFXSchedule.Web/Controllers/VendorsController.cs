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
    public class VendorsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        // GET: ReplayVendors
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
            return View(convention.Vendors.OrderBy(v => v.Title).ToList());
        }

        // GET: ReplayVendors/Details/5
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // GET: ReplayVendors/Create
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

            ViewBag.GuestIds = "";
            ViewBag.VendorTypeIDs = "";
            ViewBag.ConId = convention_id;

            return View();
        }

        // POST: ReplayVendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ExtendedDescription,Location,Image,Url")] Vendor replayVendor, HttpPostedFileBase upload, int convention_id, string categories, string guests)
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
                replayVendor.Image = azure.GetFileName(upload);
                convention.Vendors.Add(replayVendor);
                replayVendor.VendorTypes = new List<VendorType>();
                foreach (var id in categories.Split(','))
                {
                    if (int.TryParse(id, out int Id))
                    {
                        replayVendor.VendorTypes.Add(db.VendorTypes.Find(Id));
                    }
                }
                replayVendor.Guests = new List<Guest>();
                foreach (var id in guests.Split(','))
                {
                    if (int.TryParse(id, out int Id)) 
                    {
                        replayVendor.Guests.Add(db.Guests.Find(Id));
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replayVendor);
        }

        // GET: ReplayVendors/Edit/5
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConId = convention_id;
            ViewBag.VendorTypeIDs = string.Join(",", replayVendor.VendorTypes.Select(r => r.Id));

            return View(replayVendor);
        }

        private string AddType(int id, int typeId)
        {
            Vendor vend = db.Vendors.Find(id);
            VendorType vendtype = db.VendorTypes.Find(typeId);

            vend.VendorTypes.Add(vendtype);
            db.SaveChanges();

            return "success";
        }

        private string RemoveType(int id, int typeId)
        {
            Vendor vend = db.Vendors.Find(id);
            VendorType typetoremove = new VendorType();
            foreach (var item in vend.VendorTypes)
            {
                if (item.Id == typeId)
                {
                    typetoremove = item;
                }
            }
            vend.VendorTypes.Remove(typetoremove);
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

            Vendor rpe = convention.Vendors.Where(e => e.Id == id).FirstOrDefault();
            List<VendorTypeView> eventList = VendorTypeView(rpe.VendorTypes.ToList());


            return Json(eventList, JsonRequestBehavior.AllowGet);
        }

        // POST: ReplayVendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ExtendedDescription,Location,Image,Url")] Vendor replayVendor, HttpPostedFileBase upload, string image, int convention_id, string categories)
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
                var vendor = db.Vendors.Find(replayVendor.Id);
                if (upload != null)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        azure.deletefromAzure(image);
                        image = null;
                    }
                    replayVendor.Image = azure.GetFileName(upload);
                }

                vendor.Title = replayVendor.Title;
                vendor.Description = replayVendor.Description;
                vendor.ExtendedDescription = replayVendor.ExtendedDescription;
                vendor.Location = replayVendor.Location;
                vendor.Image = replayVendor.Image;
                vendor.Url = replayVendor.Url;

                SaveVendorTypes(vendor.Id, categories.Split(','));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replayVendor);
        }

        private void SaveVendorTypes(int id, string[] VendorTypeIDs)
        {
            List<int> ids = new List<int>();
            List<VendorType> typesToRemove = new List<VendorType>();
            foreach (var vendorId in VendorTypeIDs)
            {
                int i;
                if (int.TryParse(vendorId, out i))
                {
                    ids.Add(i);
                }
            }

            var vendor = db.Vendors.Find(id);
            foreach (var vendorType in vendor.VendorTypes)
            {
                if (ids.Contains(vendorType.Id))
                {
                    // keep it, remove from the ids list
                    ids.Remove(vendorType.Id);
                }
                else
                {

                    typesToRemove.Add(vendorType);
                }
            }
            foreach (var type in typesToRemove)
            {
                vendor.VendorTypes.Remove(type);
            }
            foreach (var i in ids)
            {
                vendor.VendorTypes.Add(db.VendorTypes.Find(i));
            }
        }

        // GET: ReplayVendors/Delete/5
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor == null)
            {
                return HttpNotFound();
            }
            return View(replayVendor);
        }

        // POST: ReplayVendors/Delete/5
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
            Vendor replayVendor = convention.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (replayVendor.Image != null)
            { azure.deletefromAzure(replayVendor.Image); }
            db.Vendors.Remove(replayVendor);
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

        private List<VendorTypeView> VendorTypeView(List<VendorType> baseList)
        {
            List<VendorTypeView> outList = new List<VendorTypeView>();

            foreach (var item in baseList)
            {
                VendorTypeView temp = new VendorTypeView
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
