using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    public class PublicController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Public
        public ActionResult Index(string category, DateTime? start, DateTime? end)
        {
            string result;
            if (start == null)
            {
                start = DateTime.Parse("7/1/2018");
            }
            if (end == null)
            {
                end = DateTime.Parse("1/1/2024");
            }

            if (String.IsNullOrEmpty(category))
            {
                result = JsonConvert.SerializeObject(db.Events.Where(e => e.Date >= start && e.Date <= end).OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });
            }
            else
            {
                // select all replay events where the replayeventtype.name = category
                result = JsonConvert.SerializeObject(db.Events.Where(r => r.EventTypes.Any(e => e.Name == category) && r.Date >= start && r.Date <= end).OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
            }
            return Content(result, "application/json");
        }

        // api/daily
        public ActionResult ScheduleWeb(string date, string category)
        {
            DateTime tempdate = Convert.ToDateTime(date);
            DateTime tempdate1 = tempdate.AddDays(1);
            var resultList = db.Events.Where(d => (d.Date == tempdate && string.Compare(d.StartTime, "02:00") > -1) || (tempdate1 == d.Date && string.Compare(d.StartTime, "02:00") < 0)).Where(r => r.EventTypes.Any(e => e.Name != "vendors"));
            if (!string.IsNullOrEmpty(category))
            {
                resultList = resultList.Where(r => r.EventTypes.Any(e => e.Name == category));
            }

            
            // select all replay events where the replayeventtype.name = category
            string result = JsonConvert.SerializeObject(resultList.OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            return Content(result, "application/json");
        }

        public ActionResult Categories()
        {
            var result = JsonConvert.SerializeObject(db.EventTypes.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new NoNavigationResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(result, "application/json");
        }

        public ActionResult Vendors()
        {
            var result = JsonConvert.SerializeObject(db.Vendors.OrderBy(v => v.Title).ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            return Content(result, "application/json");
        }

        public ActionResult Games(string gametype)
        {
            string result;
            if (String.IsNullOrEmpty(gametype))
            {
                result = JsonConvert.SerializeObject(db.Games.OrderBy(r => new { r.GameTitle }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });
            }
            else
            {
                // select all replay games where the replaygametype.name = gametypes
                result = JsonConvert.SerializeObject(db.Games.Where(e => e.GameType.Name == gametype).OrderBy(r =>  r.GameTitle ).ToList(), Formatting.None,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
            }
            return Content(result, "application/json");
        }

        public ActionResult AllGames(string gametype)
        {
            string result;
            if (String.IsNullOrEmpty(gametype))
            {
                result = JsonConvert.SerializeObject(db.Games.OrderBy(r => new { r.GameTitle }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });
            }
            else
            {
                // select all replay games where the replaygametype.name = gametypes
                result = JsonConvert.SerializeObject(db.Games.Where(e => e.GameType.Name == gametype).OrderBy(r => r.GameTitle).ToList(), Formatting.None,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
            }
            return Content(result, "application/json");
        }

        public ActionResult GameTypes()
        {
            var result = JsonConvert.SerializeObject(db.GameTypes.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }

        public ActionResult Locations()
        {
            var result = JsonConvert.SerializeObject(db.GameLocations.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }

        public ActionResult Search(string s)
        {
            var events = db.Events.Where(e => e.Title.ToLower().Contains(s.ToLower())).ToList();

            var results = JsonConvert.SerializeObject(events, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            return Content(results, "application/json");
        }
    }

    public class NoNavigationResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
            {
                if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal)
                {
                    prop.ShouldSerialize = obj => false;
                }
            }
            return prop;
        }
    }
}