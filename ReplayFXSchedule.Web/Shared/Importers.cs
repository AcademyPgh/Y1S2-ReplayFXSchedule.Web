using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ReplayFXSchedule.Web.Models;

namespace ReplayFXSchedule.Web.Shared
{
    public static class GoogleDownloader
    {
        static string ExtractFileId(string url)
        {
            string fileId = string.Empty;

            if (url.Contains("/file/d/"))
            {
                int startIndex = url.IndexOf("/file/d/") + "/file/d/".Length;
                int endIndex = url.IndexOf("/", startIndex);
                if (endIndex == -1)
                {
                    endIndex = url.Length;
                }
                fileId = url.Substring(startIndex, endIndex - startIndex);
            }
            else if (url.Contains("open?id="))
            {
                int startIndex = url.IndexOf("open?id=") + "open?id=".Length;
                int endIndex = url.IndexOf("&", startIndex);
                if (endIndex == -1)
                {
                    endIndex = url.Length;
                }
                fileId = url.Substring(startIndex, endIndex - startIndex);
            }

            return fileId;
        }
        public static MemoryStream Download(string url)
        {
            string fileId = ExtractFileId(url);
            string modifiedUrl = $"https://drive.google.com/uc?id={fileId}";

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    byte[] imageData = webClient.DownloadData(modifiedUrl);
                    return ProcessImageStream(new MemoryStream(imageData));
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Error downloading the image: " + ex.Message);
                }
            }
            return null;
        }

        static MemoryStream ProcessImageStream(MemoryStream stream)
        {
            try
            {
                using (Image originalImage = Image.FromStream(stream))
                {
                    int maxSize = 1024;
                    int newWidth, newHeight;
                    newWidth = originalImage.Width;
                    newHeight = originalImage.Height;

                    if (originalImage.Width > originalImage.Height)
                    {
                        newWidth = maxSize;
                        newHeight = (int)(originalImage.Height * (maxSize / (double)originalImage.Width));
                    }
                    else
                    {
                        newWidth = (int)(originalImage.Width * (maxSize / (double)originalImage.Height));
                        newHeight = maxSize;
                    }

                    using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        MemoryStream resizedImageStream = new MemoryStream();
                        resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        resizedImageStream.Position = 0;
                        return resizedImageStream;
                    }
                }
            }
                        catch (Exception ex)
            {
                Console.WriteLine("Error processing the image, most likely heic: " + ex.Message);
                return null;
            }
        }

    }

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
            methods.Add(CleanTikTok);

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

        private string CleanTikTok()
        {
            string url = GetStringValue("TikTok");
            if (url.Length > 0)
            {
                url = GetUsername(url, "tiktok.com/");
                return Clean($"tiktok.com/@{url}");
            }
            return "";
        }

        private string GetUsername(string url, string domain)
        {
            if (url.Length > 0)
            {
                int index = url.ToLower().IndexOf(domain);
                if (index > -1)
                {
                    url = url.Substring(index + domain.Length);
                }
                index = url.IndexOf("?");
                if (index > -1)
                {
                    url = url.Substring(0, index);
                }

                url = url.Replace("@", "");
                return url;
            }
            return "";
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
                url = GetUsername(url, "twitter.com/");
                return Clean($"twitter.com/{url}");
            }
            return "";
        }

        private string CleanInstagram()
        {
            string url = GetStringValue("Instagram");
            if (url.Length > 0)
            {
                url = GetUsername(url, "instagram.com/");
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
            currentEvent.EndTime = GetStringValue("EndTime").Replace(": ", " ");
            currentEvent.Title = GetStringValue("Title");
            currentEvent.StartTime = GetStringValue("StartTime").Replace(": ", " ");
            currentEvent.ExtendedDescription = GetStringValue("ExtendedDescription");
            if (!String.IsNullOrEmpty(GetStringValue("GoogleImage")))
            {
                currentEvent.Image = GetStringValue("GoogleImage");
            }


            currentEvent.URL = GetUrls();

            List<int> eventTypeIds = (GetStringValue("EventTypes") + "," + GetStringValue("GenreTypes")).Split(',').Where(s => int.TryParse(s, out int i)).Select(s => Convert.ToInt32(s)).ToList();
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