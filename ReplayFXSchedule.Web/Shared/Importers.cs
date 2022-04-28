using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReplayFXSchedule.Web.Models;

namespace ReplayFXSchedule.Web.Shared
{
    public class EventImporter
    {
        private Dictionary<string, int> FieldOrder;
        private string[] Values;

        delegate String UrlGetter();

        public EventImporter(string[] fields)
        {
            FieldOrder = new Dictionary<string, int>();
            for (int i = 0; i < fields.Length; i++)
            {
                if(!String.IsNullOrEmpty(fields[i]))
                {
                    FieldOrder.Add(fields[i], i);
                }
            }
        }
        
        private string GetStringValue(string fieldName)
        {
            if(FieldOrder.ContainsKey(fieldName))
            {
                return Values[FieldOrder[fieldName]];
            }
            return "";
        }

        private string GetUrls()
        {
            List<string> urls = new List<string>();
            List<UrlGetter> methods = new List<UrlGetter>();
            methods.Add(CleanURL);
            methods.Add(CleanFacebook);
            methods.Add(CleanTwitter);
            methods.Add(CleanInstagram);

            foreach(var method in methods)
            {
                string temp;
                temp = method();
                if (!String.IsNullOrEmpty(temp))
                {
                    urls.Add(temp);
                }
            }
            
            return String.Join(",", urls);
        }



        private string Clean(string url)
        {
            url = url.Replace("http://", "");
            url = url.Replace("https://", "");
            if (url.Length > 0)
            {
                return "https://" + url;
            }
            return "";
        }

        private string CleanURL()
        {
            string url = GetStringValue("URL");
            return Clean(url);
        }

        private string CleanFacebook()
        {
            string url = GetStringValue("Facebook");
            return Clean(url);
        }

        private string CleanTwitter()
        {
            string url = GetStringValue("Twitter");
            if (url.Length > 0)
            {
                if (url.Contains("twitter.com"))
                {
                    return Clean(url);
                }

                url = url.Replace("@", "");
                return Clean($"twitter.com/{url}");
            }
            return "";
        }

        private string CleanInstagram()
        {
            string url = GetStringValue("Instagram");
            if (url.Length > 0)
            {
                if (url.Contains("instagram.com"))
                {
                    return Clean(url);
                }
                url = url.Replace("@", "");
                return Clean($"instagram.com/{url}");
            }
            return "";
        }

        public Event EventFactory(string[] values, ReplayFXDbContext context)
        {
            Values = values;
            Event currentEvent = new Event();
            currentEvent.Date = DateTime.Parse(GetStringValue("Date"));
            currentEvent.Description = GetStringValue("Description");
            currentEvent.EndTime = GetStringValue("EndTime");
            currentEvent.Title = GetStringValue("Title");
            currentEvent.StartTime = GetStringValue("StartTime");
            currentEvent.ExtendedDescription = GetStringValue("ExtendedDescription");



            currentEvent.URL = GetUrls();

            List<int> eventTypeIds = GetStringValue("EventTypes").Split(',').Where(s => int.TryParse(s, out int i)).Select(s => Convert.ToInt32(s)).ToList();
            currentEvent.EventTypes = context.EventTypes.Where(et => eventTypeIds.Contains(et.Id)).ToList();

            if(int.TryParse(GetStringValue("EventLocation"), out int locationId))
            {
                currentEvent.EventLocation = context.GameLocations.Where(l => l.Id == locationId).FirstOrDefault();
            }
            
            // currentEvent.EventLocation = values[FieldOrder["EventLocation"]];
            // currentEvent.EventTypes = values[FieldOrder["EventTypes"]];
            // Title
            // StartTime
            // EndTime
            // ExtendedDescription
            // EventLocation
            // URL
            // EventTypes


            return currentEvent;
        }
    }
}