using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Text;

namespace ReplayFXSchedule.Web.Controllers
{
    [RoutePrefix("api/v2")]
    public class APIV2Controller : ApiController
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private UserService us;

        //// GET: Public
        //[Route("convention/{convention_id}")]
        //[HttpGet]
        //public async Task<HttpResponseMessage> Index(int convention_id)
        //{
        //    var cache = db.Cache.Where(c => c.ConventionId == convention_id).OrderByDescending(c => c.LastRun).FirstOrDefault();
        //    string convention = "";
        //    if(cache == null)
        //    {
        //        cache = await UpdateCache(convention_id, null);
        //    }
        //    if (cache.LastRun < DateTime.Now.AddMinutes(-2))
        //    {
        //        UpdateCache(convention_id, cache.ApiResult);
        //    }

        //    convention = cache.ApiResult;
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    response.Content = new StringContent(convention, Encoding.UTF8, "application/json");
        //    return response;
        //}


        [Route("convention/{convention_id}")]
        [HttpGet]
        public async Task<Convention> Index(int convention_id)
        {
            Convention convention = db.Conventions.Find(convention_id);
            var showPrivate = isVip(convention);
            convention.Events = GetEvents(convention, showPrivate);
            convention.EventTypes = GetEventTypes(convention, showPrivate);
            convention.Vendors = convention.Vendors.OrderBy(e => e.Title).ToList();
            convention.VendorTypes = convention.VendorTypes.OrderBy(e => e.Name).ToList();
            convention.Games = convention.Games.Where(g => g.AtConvention).OrderBy(g => g.GameTitle).ToList();
            convention.Guests = convention.Guests.OrderBy(e => e.Name).ToList();
            convention.GuestTypes = convention.GuestTypes.OrderBy(e => e.Name).ToList();
            convention.Sponsors = convention.Sponsors.OrderBy(e => e.Name).ToList();

            convention.Menu = GetMenus(convention_id);
            return convention;
        }

        public async Task<GarbageCache> UpdateCache(int convention_id, string old_api)
        {
            var cachedelete = db.Cache.Where(c => c.LastRun < DateTime.Now.AddMinutes(-10));
            db.Cache.RemoveRange(cachedelete);
            db.SaveChanges();

            var cache = new GarbageCache
            {
                ConventionId = convention_id,
                ApiResult = old_api,
                LastRun = DateTime.Now
            };

            db.Cache.Add(cache);
            if (!String.IsNullOrEmpty(old_api))
            {
                db.SaveChanges();
            }

            Convention convention = db.Conventions.Find(convention_id);
            var showPrivate = isVip(convention);
            convention.Events = GetEvents(convention, showPrivate);
            convention.EventTypes = GetEventTypes(convention, showPrivate);
            convention.Vendors = convention.Vendors.OrderBy(e => e.Title).ToList();
            convention.VendorTypes = convention.VendorTypes.OrderBy(e => e.Name).ToList();
            convention.Games = convention.Games.Where(g => g.AtConvention).OrderBy(g => g.GameTitle).ToList();
            convention.Guests = convention.Guests.OrderBy(e => e.Name).ToList();
            convention.GuestTypes = convention.GuestTypes.OrderBy(e => e.Name).ToList();
            convention.Sponsors = convention.Sponsors.OrderBy(e => e.Name).ToList();

            convention.Menu = GetMenus(convention_id);

            cache.ApiResult = JsonConvert.SerializeObject(convention, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
            cache.LastRun = DateTime.Now;
            db.SaveChanges();
            return cache;
        }

        [Route("ClearCache")]
        [HttpGet]
        public IHttpActionResult ClearCache()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [GarbageCaches]");
            return Ok("done");
        }

        private List<Menu> GetMenus(int id)
        {
            List<Menu> menu = new List<Menu>();

            if (id == 15)
            {
                menu.Add(new Menu { Type = "Schedule", Title = "Schedule" });
                MenuOption tempOption = new MenuOption { Title = "My Schedule", ScheduleFilter = "my-schedule" };
                menu.Add(new Menu { Type = "Schedule", Title = "My Schedule", Options = tempOption });
                menu.Add(new Menu { Type = "VendorMenu" });
                menu.Add(new Menu { Type = "EventMenu" });
                menu.Add(new Menu { Type = "Sponsors", Title = "Sponsors" });
            }
            else if (id == 16)
            {
                menu.Add(new Menu { Type = "Schedule", Title = "Schedule" });
                MenuOption tempOption = new MenuOption { Title = "My Schedule", ScheduleFilter = "my-schedule" };
                menu.Add(new Menu { Type = "Schedule", Title = "My Schedule", Options = tempOption });
                menu.Add(new Menu { Type = "VendorMenu" });
                menu.Add(new Menu { Type = "EventMenu" });
                menu.Add(new Menu { Type = "Sponsors", Title = "Sponsors" });
            }
            else if (id == 19) // Pittsburgh Robotics
            {
                menu.Add(new Menu { Type = "TabSchedule", Title = "Schedule" });
                MenuOption tempOption = new MenuOption { Title = "My Schedule", ScheduleFilter = "my-schedule" };
                menu.Add(new Menu { Type = "Schedule", Title = "My Schedule", Options = tempOption });
                menu.Add(new Menu { Type = "EventMenu" });
                tempOption = new MenuOption { Title = "Exhibitors" };
                menu.Add(new Menu { Type = "VendorsList", Title = "Exhibitors", Options = tempOption });
                menu.Add(new Menu { Type = "Sponsors", Title = "Sponsors" });
                menu.Add(new Menu { Type = "StaticMap", Title = "Map" });
            }
            else if (id == 11) // Millvale Music Festival
            {
                menu.Add(new Menu { Type = "TabSchedule", Title = "Schedule" });
                MenuOption tempOption = new MenuOption { Title = "My Schedule", ScheduleFilter = "my-schedule" };
                menu.Add(new Menu { Type = "Schedule", Title = "My Schedule", Options = tempOption });
                tempOption = new MenuOption { Title = "ACTS" };
                menu.Add(new Menu { Type = "GuestsList", Title = "Acts" , Options = tempOption});
                menu.Add(new Menu { Type = "EventMenu" });
                tempOption = new MenuOption { Title = "VISUAL ARTISTS" };
                menu.Add(new Menu { Type = "VendorsList", Title = "Visual Artists", Options = tempOption });
                menu.Add(new Menu { Type = "Sponsors", Title = "Sponsors" });
                menu.Add(new Menu { Type = "StaticMap", Title = "Map" });
            }
            else
            {
                menu.Add(new Menu { Type = "Schedule", Title = "Schedule" });
                MenuOption tempOption = new MenuOption { Title = "My Schedule", ScheduleFilter = "my-schedule" };
                menu.Add(new Menu { Type = "Schedule", Title = "My Schedule", Options = tempOption });
                menu.Add(new Menu { Type = "EventMenu" });
                menu.Add(new Menu { Type = "VendorsList", Title = "Vendors" });
                menu.Add(new Menu { Type = "Sponsors", Title = "Sponsors" });
                menu.Add(new Menu { Type = "StaticMap", Title = "Map" });
            }

            return menu;
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

        [Route("guesttypes/{convention_id}")]
        [HttpGet]
        public List<GuestType> GuestTypes(int convention_id)
        {
            var guestTypes = db.Conventions.Find(convention_id).GuestTypes.ToList();
            return guestTypes;
        }

        [Route("vendortypes/{convention_id}")]
        [HttpGet]
        public List<VendorType> VendorTypes(int convention_id)
        {
            var vendorTypes = db.Conventions.Find(convention_id).VendorTypes.ToList();
            return vendorTypes;
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

        [Route("convention/{convention_id}/messages/{date_time}")]
        public List<DisplayMessage> GetMessagesByDate(int convention_id, DateTime date_time)
        {
            var convention = db.Conventions.Find(convention_id);
            if (date_time < convention.StartDate || date_time > convention.EndDate.AddDays(1))
            {
                date_time = convention.StartDate;
            }
            return GetMessages(convention).Where(e => e.Date == date_time).ToList();
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

        [Route("convention/{convention_id}/games/{gametype?}")]
        public List<Game> GetGames(int convention_id, string gametype)
        {
            var convention = db.Conventions.Find(convention_id);
            var games = convention.Games.Where(g => String.IsNullOrEmpty(gametype) || g.GameType.Name == gametype).ToList();
            return games;
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

        private List<DisplayMessage> GetMessages(Convention convention)
        {
            return convention.DisplayMessages.OrderBy(e => e.Date).ThenBy(e => e.StartTime).ThenBy(e => e.Title).ToList();
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
            return convention.EventTypes.Where(e => e.IsPrivate == false || e.IsPrivate == showPrivate).OrderBy(e => e.Name).ToList();
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

    }
}