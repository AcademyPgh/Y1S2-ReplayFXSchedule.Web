using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ReplayFXSchedule.Web.Controllers
{
    [RoutePrefix("api/v2")]
    public class APIV2Controller : ApiController
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private UserService us;

        // GET: Public
        [Route("convention/{convention_id}")]
        [HttpGet]
        public IHttpActionResult Index(int convention_id)
        {
            Convention convention = db.Conventions.Find(convention_id);
            var showPrivate = isVip(convention);
            convention.Events = GetEvents(convention, showPrivate);
            convention.EventTypes = GetEventTypes(convention, showPrivate);
            convention.Vendors = convention.Vendors.OrderBy(e => e.Title).ToList();
            convention.Games = convention.Games.Where(g => g.AtConvention).OrderBy(g => g.GameTitle).ToList();
            return Ok(convention);
        }

        [Route("conventions")]
        [HttpGet]
        public IHttpActionResult Conventions()
        {
            var output = db.Conventions.OrderBy(c => new { c.StartDate }).Where(c => c.EnableInApp == true).Select(c => new ConventionViewModel()
            {
                Id = c.Id,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Name = c.Name,
                Address = c.Address,
                Address2 = c.Address2,
                City = c.City,
                State = c.State,
                Zip = c.Zip,
                Url = c.Url,
                TicketUrl = c.TicketUrl,
                HeaderImage = c.HeaderImage,
                Hashtag = c.Hashtag
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

        [Route("locations/{convention_id}")]
        [HttpGet]
        public List<GameLocation> Locations(int convention_id)
        {
            var gameLocations = db.Conventions.Find(convention_id).GameLocations.ToList();
            return gameLocations;
        }

        [Route("convention/{convention_id}/feed")]
        [HttpGet]
        public List<Post> Feed(int convention_id)
        {
            var feed = db.Posts.Where(p => p.Convention.Id == convention_id && p.Viewable == true).OrderByDescending(p => p.PostedOn).ToList();
            return feed;
        }


        [Authorize]
        [Route("convention/{convention_id}/add")]
        [HttpPost]
        public List<Post> AddPost(int convention_id, PostUpload post)
        {
            var convention = db.Conventions.Find(convention_id);
            if (convention == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (String.IsNullOrEmpty(post.Text))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            // var user = db.AppUsers.Find(4);
            var dbpost = new Post()
            {
                Text = post.Text,
                PostedOn = DateTime.Now,
                User = user,
                Convention = convention,
                Viewable = true
            };
            db.Posts.Add(dbpost);
            db.SaveChanges();
            return Feed(convention_id);
        }

        [Route("convention/{convention_id}/events/{date_time}")]
        public List<Event> GetEventsByDate(int convention_id, DateTime date_time)
        {
            var convention = db.Conventions.Find(convention_id);
            if(date_time < convention.StartDate || date_time > convention.EndDate.AddDays(1))
            {
                date_time = convention.StartDate;
            }
            return GetEvents(convention).Where(e => e.Date == date_time).ToList();
        }

        [Route("convention/{convention_id}/events/{date_time}/{location}")]
        public List<Event> GetEventsByLocation(int convention_id, DateTime date_time, int location)
        {
            var convention = db.Conventions.Find(convention_id);
            if (date_time < convention.StartDate || date_time > convention.EndDate.AddDays(1))
            {
                date_time = convention.StartDate;
            }
            var events = GetEvents(convention).Where(e => e.Date == date_time);
            return events.Where(e => e.EventLocation != null && e.EventLocation.Id == location).ToList();
        }

        private bool isVip(Convention convention)
        {
            if (User.Identity.IsAuthenticated)
            {
                us = new UserService((ClaimsIdentity)User.Identity, db);
                var user = us.GetUser();
                if (user.AppUserPermissions.Any(a => a.Convention.Id == convention.Id && a.UserRole != UserRole.User))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Event> GetEvents(Convention convention, bool? showPrivate = null)
        {
            if(showPrivate == null)
            {
                showPrivate = isVip(convention);
            }
            return convention.Events.Where(e => e.IsPrivate == false || e.IsPrivate == showPrivate).OrderBy(e => e.Date).ThenBy(e => e.StartTime).ThenBy(e => e.Title).ToList();
        }

        private List<EventType> GetEventTypes(Convention convention, bool? showPrivate = null)
        {
            if (showPrivate == null)
            {
                showPrivate = isVip(convention);
            }
            return convention.EventTypes.Where(e => e.IsPrivate == false || e.IsPrivate == showPrivate).ToList();
        }

        [Route("convention/{convention_id}/email")]
        [HttpPost]
        public IHttpActionResult SubmitEmail(int convention_id, PostUpload post)
        {
            var con = db.Conventions.Find(convention_id);
            if(con == null)
            {
                return BadRequest();
            }
            var email = new UserEmail()
            {
                Convention = con,
                Email = post.Text,
                DateSubmitted = DateTime.Now
            };
            db.UserEmails.Add(email);
            db.SaveChanges();
            return Ok();
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