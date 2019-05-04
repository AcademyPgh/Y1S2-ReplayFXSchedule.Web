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
    public class ConventionsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private AzureTools azure = new AzureTools();

        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }

        // GET: Conventions
        public ActionResult Index()
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            var conventions = db.AppUserPermissions.Where(aup => aup.AppUser.Id == user.Id && aup.UserRole == UserRole.Admin).Select(x => x.Convention).ToList();
            ViewBag.isAdmin = user.isSuperAdmin;
            return View(conventions);
        }

        // GET: Conventions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // GET: Conventions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartDate,EndDate,Name,Address,Address2,City,State,Zip,HeaderImage,Hashtag,MapImage,EnableInApp,TicketUrl,Url,TrackingUrl,AppUrl")] Convention convention, HttpPostedFileBase headerImageFile, HttpPostedFileBase mapImageFile)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            if (!user.isSuperAdmin)
            {
                return new HttpNotFoundResult();
            }
            if (ModelState.IsValid)
            {
                if (headerImageFile != null)
                {
                    convention.HeaderImage = azure.GetFileName(headerImageFile);
                }
                if (mapImageFile != null)
                {
                    convention.MapImage = azure.GetFileName(mapImageFile);
                }

                db.Conventions.Add(convention);
                db.AppUserPermissions.Add(new AppUserPermission { AppUser = user, UserRole = UserRole.Admin, Convention = convention });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(convention);
        }

        // GET: Conventions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // POST: Conventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate,Name,Address,Address2,City,State,Zip,HeaderImage,Hashtag,MapImage,EnableInApp,TicketUrl,Url,TrackingUrl,AppUrl")] Convention convention, HttpPostedFileBase headerImageFile, HttpPostedFileBase mapImageFile)
        {
            if (ModelState.IsValid)
            {
                var con = db.Conventions.Where(g => g.Id == convention.Id).FirstOrDefault();
                var deleted = false;
                if (con.HeaderImage != convention.HeaderImage)
                {
                    if (!string.IsNullOrEmpty(con.HeaderImage))
                    {
                        azure.deletefromAzure(con.HeaderImage);
                        con.HeaderImage = null;
                        deleted = true;
                    }
                }
                if (headerImageFile != null)
                {
                    if (!deleted & !string.IsNullOrEmpty(con.HeaderImage))
                    {
                        azure.deletefromAzure(con.HeaderImage);
                        con.HeaderImage = null;
                        deleted = true;
                    }
                    convention.HeaderImage = azure.GetFileName(headerImageFile);
                }

                deleted = false;
                if (con.MapImage != convention.MapImage)
                {
                    if (!string.IsNullOrEmpty(con.MapImage))
                    {
                        azure.deletefromAzure(con.MapImage);
                        con.MapImage = null;
                        deleted = true;
                    }
                }
                if (mapImageFile != null)
                {
                    if (!deleted & !string.IsNullOrEmpty(con.MapImage))
                    {
                        azure.deletefromAzure(con.MapImage);
                        con.MapImage = null;
                        deleted = true;
                    }
                    convention.MapImage = azure.GetFileName(mapImageFile);
                }

                con.Name = convention.Name;
                con.StartDate = convention.StartDate;
                con.EndDate = convention.EndDate;
                con.EnableInApp = convention.EnableInApp;
                con.Address = convention.Address;
                con.Address2 = convention.Address2;
                con.City = convention.City;
                con.State = convention.State;
                con.Zip = convention.Zip;
                con.TicketUrl = convention.TicketUrl;
                con.Url = convention.Url;
                con.Hashtag = convention.Hashtag;
                con.HeaderImage = convention.HeaderImage;
                con.MapImage = convention.MapImage;
                con.TrackingUrl = convention.TrackingUrl;
                con.AppUrl = convention.AppUrl;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(convention);
        }

        // GET: Conventions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convention convention = db.Conventions.Find(id);
            if (convention == null)
            {
                return HttpNotFound();
            }
            return View(convention);
        }

        // POST: Conventions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Convention convention = db.Conventions.Find(id);
            if (!string.IsNullOrEmpty(convention.HeaderImage))
            {
                azure.deletefromAzure(convention.HeaderImage);
            }
            if (!string.IsNullOrEmpty(convention.MapImage))
            {
                azure.deletefromAzure(convention.MapImage);
            }
            db.AppUserPermissions.RemoveRange(convention.AppUserPermissions);

            db.Conventions.Remove(convention);
            db.SaveChanges();
            return Redirect("/");
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
