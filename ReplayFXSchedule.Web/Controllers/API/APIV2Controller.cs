using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ReplayFXSchedule.Web.Controllers
{
    [RoutePrefix("api/v2")]
    public class APIV2Controller : ApiController
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Public
        [Route("convention/{convention_id}")]
        [HttpGet]
        public IHttpActionResult Index(int? convention_id = null)
        {
            Convention convention = db.Conventions.Find(convention_id);
            return Ok(convention);
        }

        [Route("conventions")]
        [HttpGet]
        public IHttpActionResult Conventions()
        {
            var output = db.Conventions.Select(c => new ConventionViewModel()
                {
                    Id = c.Id,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Name = c.Name,
                    Address = c.Address,
                    Address2 = c.Address2,
                    City = c.City,
                    State = c.State,
                    Zip = c.Zip
                }).ToList();
            return Ok(output);
        }

        //[Authorize]
        //[Route("testing")]
        //[HttpGet]
        //public string Testing()
        //{
        //    var user_service = new UserService((ClaimsIdentity)User.Identity,db);
        //    return "workin";
        //}

        [Route("schedule/{convention_id}/{date}")]
        [HttpGet]
        public IHttpActionResult GetDaySchedule(int convention_id, DateTime date)
        {
            var events = db.Events.Where(re => re.Convention.Id == convention_id && re.Date == date).ToList();
            return Ok(events);
        }

        [Route("categories/{convention_id}")]
        [HttpGet]
        public List<EventType> Categories(int convention_id)
        {
            var eventTypes = db.Conventions.Find(convention_id).EventTypes.ToList();
            return eventTypes;
        }

        //public ActionResult Conferences()
        //{
        //    string result = JsonConvert.SerializeObject(db.ReplayConventions.ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //            ContractResolver = new CamelCasePropertyNamesContractResolver()
        //        });
        //    return Content(result, "application/json");
        //}

        //// api/daily
        //public ActionResult ScheduleWeb(int? convention_id, string date, string category)
        //{
        //    DateTime tempdate = Convert.ToDateTime(date);
        //    DateTime tempdate1 = tempdate.AddDays(1);
        //    var resultList = db.ReplayEvents.Where(d => (convention_id == null || d.Convention.Id == convention_id) && (d.Date == tempdate && string.Compare(d.StartTime, "02:00") > -1) || (tempdate1 == d.Date && string.Compare(d.StartTime, "02:00") < 0)).Where(r => r.ReplayEventTypes.Any(e => e.Name != "vendors"));
        //    if (!string.IsNullOrEmpty(category))
        //    {
        //        resultList = resultList.Where(r => r.ReplayEventTypes.Any(e => e.Name == category));
        //    }


        //    // select all replay events where the replayeventtype.name = category
        //    string result = JsonConvert.SerializeObject(resultList.OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
        //    new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //    });

        //    return Content(result, "application/json");
        //}

        //public ActionResult Categories(int? convention_id)
        //{
        //    var result = JsonConvert.SerializeObject(db.ReplayEventTypes.Where(e => e.Convention != null || e.Convention.Id == convention_id).ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });

        //    return Content(result, "application/json");
        //}

        //public ActionResult Vendors(int? convention_id)
        //{
        //    var result = JsonConvert.SerializeObject(db.ReplayVendors.Where(e => e.Convention != null || e.Convention.Id == convention_id).OrderBy(v => v.Title).ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //            ContractResolver = new CamelCasePropertyNamesContractResolver()
        //        });

        //    return Content(result, "application/json");
        //}

        //public ActionResult Games(int? convention_id, string gametype)
        //{
        //    string result;
        //    result = JsonConvert.SerializeObject(db.ReplayGames.Where(e => (e.Convention == null || e.Convention.Id == convention_id) && (String.IsNullOrEmpty(gametype) || e.ReplayGameType.Name == gametype)).OrderBy(r => r.GameTitle).ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //            ContractResolver = new CamelCasePropertyNamesContractResolver()
        //        });
        //    return Content(result, "application/json");
        //}

        //public ActionResult AllGames(int? convention_id, string gametype)
        //{
        //    string result;
        //    if (String.IsNullOrEmpty(gametype))
        //    {
        //        result = JsonConvert.SerializeObject(db.ReplayGames.OrderBy(r => new { r.GameTitle }).ToList(), Formatting.None,
        //               new JsonSerializerSettings
        //               {
        //                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //                   ContractResolver = new CamelCasePropertyNamesContractResolver()
        //               });
        //    }
        //    else
        //    {
        //        // select all replay games where the replaygametype.name = gametypes
        //        result = JsonConvert.SerializeObject(db.ReplayGames.Where(e => e.ReplayGameType.Name == gametype).OrderBy(r => r.GameTitle).ToList(), Formatting.None,
        //                new JsonSerializerSettings
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //                    ContractResolver = new CamelCasePropertyNamesContractResolver()
        //                });
        //    }
        //    return Content(result, "application/json");
        //}

        //public ActionResult GameTypes(int? convention_id)
        //{
        //    var result = JsonConvert.SerializeObject(db.ReplayGameTypes.Where(e => e.Convention != null || e.Convention.Id == convention_id).ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });
        //    return Content(result, "application/json");
        //}

        //public ActionResult Locations(int? convention_id)
        //{
        //    var result = JsonConvert.SerializeObject(db.ReplayGameLocations.Where(e => e.Convention != null || e.Convention.Id == convention_id).ToList(), Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });
        //    return Content(result, "application/json");
        //}

        //public ActionResult Search(int? convention_id, string s)
        //{
        //    var events = db.ReplayEvents.Where(e => (e.Convention != null || e.Convention.Id == convention_id) && e.Title.ToLower().Contains(s.ToLower())).ToList();

        //    var results = JsonConvert.SerializeObject(events, Formatting.None,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //            ContractResolver = new CamelCasePropertyNamesContractResolver()
        //        });
        //    return Content(results, "application/json");
        //}
    }
}