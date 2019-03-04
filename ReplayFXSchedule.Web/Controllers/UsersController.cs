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
    public class UsersController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.AppUsers.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.Conventions = db.Conventions.ToList();
            ViewBag.UserRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList();
            return View(appUser);
        }

        public ActionResult RemovePermissions(int id, int perm_id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            if (user.isSuperAdmin)
            {
                var perm = db.AppUserPermissions.Find(perm_id);
                db.AppUserPermissions.Remove(perm);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id });
        }

        public ActionResult AddPermissions(int id, int convention_id, UserRole role)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            if (user.isSuperAdmin)
            {
                var permUser = db.AppUsers.Find(id);
                var con = db.Conventions.Find(convention_id);
                if (db.AppUserPermissions.Where(a => a.UserRole == role && a.AppUser.Id == id && a.Convention.Id == convention_id).ToList().Count == 0)
                {
                    var perm = new AppUserPermission() { AppUser = permUser, Convention = con, UserRole = role };
                    db.AppUserPermissions.Add(perm);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id });
        }

        public ActionResult ToggleSuperAdmin(int id)
        {
            var us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            if (user.isSuperAdmin)
            {
                var permUser = db.AppUsers.Find(id);
                if(permUser != null)
                {
                    permUser.isSuperAdmin = !permUser.isSuperAdmin;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id });
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Email,DisplayName,Auth0,isSuperAdmin")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,DisplayName,Auth0,isSuperAdmin")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            db.AppUsers.Remove(appUser);
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
