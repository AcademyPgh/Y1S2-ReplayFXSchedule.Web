using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;

namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class PhotoProcessQueuesController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: PhotoProcessQueues
        public ActionResult Index()
        {
            return View(db.PhotoProcessQueue.ToList());
        }

        // GET: PhotoProcessQueues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProcessQueue photoProcessQueue = db.PhotoProcessQueue.Find(id);
            if (photoProcessQueue == null)
            {
                return HttpNotFound();
            }
            return View(photoProcessQueue);
        }

        // GET: PhotoProcessQueues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoProcessQueues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EventId,URL,Status,Error,Created,Processed")] PhotoProcessQueue photoProcessQueue)
        {
            if (ModelState.IsValid)
            {
                db.PhotoProcessQueue.Add(photoProcessQueue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photoProcessQueue);
        }

        // GET: PhotoProcessQueues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProcessQueue photoProcessQueue = db.PhotoProcessQueue.Find(id);
            if (photoProcessQueue == null)
            {
                return HttpNotFound();
            }
            return View(photoProcessQueue);
        }

        // POST: PhotoProcessQueues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventId,URL,Status,Error,Created,Processed")] PhotoProcessQueue photoProcessQueue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoProcessQueue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photoProcessQueue);
        }

        // GET: PhotoProcessQueues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProcessQueue photoProcessQueue = db.PhotoProcessQueue.Find(id);
            if (photoProcessQueue == null)
            {
                return HttpNotFound();
            }
            return View(photoProcessQueue);
        }

        // POST: PhotoProcessQueues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoProcessQueue photoProcessQueue = db.PhotoProcessQueue.Find(id);
            db.PhotoProcessQueue.Remove(photoProcessQueue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveAll()
        {
            var all = db.PhotoProcessQueue.ToList();
            db.PhotoProcessQueue.RemoveRange(all);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ProcessAll()
        {
            var all = db.PhotoProcessQueue.Where(ppq => ppq.Status == PhotoProcessQueueStatus.New).ToList();
            var az = new AzureTools();

            foreach (var item in all)
            {
                item.Status = PhotoProcessQueueStatus.Processing;
                db.SaveChanges();
                var stream = GoogleDownloader.Download(item.URL);
                if(stream == null)
                {
                    item.Status = PhotoProcessQueueStatus.Error;
                    item.Error = "Could not download image";
                    db.SaveChanges();
                    continue;
                }
                var filename = az.GetFileName(stream);
                var rpe = db.Events.Find(item.EventId);
                rpe.Image = filename;
                item.Processed = DateTime.Now;
                item.Status = PhotoProcessQueueStatus.Processed;
                db.SaveChanges();
            }
            // db.SaveChanges();
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
