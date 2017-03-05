﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReplayFXSchedule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplayFXSchedule.Web.Controllers
{
    public class PublicController : Controller
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();

        // GET: Public
        public ActionResult Index(string category)
        {
            string result;
            if (String.IsNullOrEmpty(category))
            {
                result = JsonConvert.SerializeObject(db.ReplayEvents.OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });
            }
            else
            {
                // select all replay events where the replayeventtype.name = category
                result = JsonConvert.SerializeObject(db.ReplayEvents.Where(r => r.ReplayEventTypes.Any(e => e.Name == category)).OrderBy(r => new { r.Date, r.StartTime }).ToList(), Formatting.None,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
            }
            return Content(result, "application/json");
        }

        public ActionResult Categories()
        {
            var result = JsonConvert.SerializeObject(db.ReplayEventTypes.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(result, "application/json");
        }

        public ActionResult Games(string gametype)
        {
            string result;
            if (String.IsNullOrEmpty(gametype))
            {
                result = JsonConvert.SerializeObject(db.ReplayGames.OrderBy(r => new { r.GameTitle }).ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new CamelCasePropertyNamesContractResolver()
                       });
            }
            else
            {
                // select all replay games where the replaygametype.name = gametypes
                result = JsonConvert.SerializeObject(db.ReplayGames.Where(e => e.ReplayGameType.Name == gametype).OrderBy(r =>  r.GameTitle ).ToList(), Formatting.None,
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
            var result = JsonConvert.SerializeObject(db.ReplayGameTypes.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }
        public ActionResult Locations()
        {
            var result = JsonConvert.SerializeObject(db.ReplayGameLocations.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }
    }
}