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
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace ReplayFXSchedule.Web.Controllers
{
    [Authorize]
    public class ReplayEventsController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: ReplayEvents
        public ActionResult Index(string search)
        {
            //  return View(db.ReplayEvents.Where(x => x.Title.StartsWith(search)|| search == null).ToList());
            return View(db.ReplayEvents.OrderBy(x => new { x.Date, x.StartTime }).ToList());
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
            ViewBag.ReplayEventTypeIDs = "";
            return View();
        }

        // POST: ReplayEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image")] ReplayEvent replayEvent, string categories, HttpPostedFileBase upload)
        {
            int indexExt = 0;
            string ext = "";
            if (ModelState.IsValid)
            {
                if (upload !=null)
                {
                    indexExt = upload.FileName.IndexOf(".");
                    ext = upload.FileName.Substring(indexExt);
                    string eventimgname = Guid.NewGuid() + ext;
                    replayEvent.Image = eventimgname;
                    uploadtoAzure(eventimgname, upload);
                }
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

        private void uploadtoAzure(string filename, HttpPostedFileBase upload)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            // Retrieve reference to a blob named "someimage.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{filename}");
            // Create or overwrite the "someimage.jpg" blob with contents from an upload stream.
            blockBlob.UploadFromStream(upload.InputStream);
        }
        private void deletefromAzure(string filename)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            // Retrieve reference to a blob named "someimage.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{filename}");
            // Create or overwrite the "someimage.jpg" blob with contents from an upload stream.
            if (blockBlob.Exists())
            {
                blockBlob.Delete();
            }
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

            ViewBag.ReplayEventTypeIDs = string.Join(",", replayEvent.ReplayEventTypes.Select(r => r.Id));
            return View(replayEvent);
        }

        private string AddType(int id, int typeId)
        {
            ReplayEvent rpe = db.ReplayEvents.Find(id);
            ReplayEventType rpet = db.ReplayEventTypes.Find(typeId);

            rpe.ReplayEventTypes.Add(rpet);
            db.SaveChanges();

            return "success";
        }

        private string RemoveType(int id, int typeId)
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
        public ActionResult Edit([Bind(Include = "Id,Title,Date,StartTime,EndTime,Description,ExtendedDescription,Location,Image")] ReplayEvent replayEvent, string categories, HttpPostedFileBase upload, string image)
        {
            int indexExt = 0;
            string ext = "";
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        deletefromAzure(image);
                        image = null;
                    }
                    indexExt = upload.FileName.IndexOf(".");
                    ext = upload.FileName.Substring(indexExt);
                    string imagename = Guid.NewGuid() + ext;
                    replayEvent.Image = imagename;
                    uploadtoAzure(imagename, upload);
                }
                //Modified entity state causes us to not be able to update connected replayeeventtypes
                ReplayEvent rpe = db.ReplayEvents.Find(replayEvent.Id);

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
            List<ReplayEventType> typesToRemove = new List<ReplayEventType>();
            foreach (var eventId in EventTypeIDs)
            {
                int i;
                if (int.TryParse(eventId, out i))
                {
                    ids.Add(i);
                }
            }

            var replayEvent = db.ReplayEvents.Find(id);
            foreach (var eventType in replayEvent.ReplayEventTypes)
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
                replayEvent.ReplayEventTypes.Remove(type);
            }
            foreach (var i in ids)
            {
                replayEvent.ReplayEventTypes.Add(db.ReplayEventTypes.Find(i));
            }
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
