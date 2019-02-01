using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class AppUserView
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
    }

    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Auth0 { get; set; }
        public string Image { get; set; }
        
        public bool isSuperAdmin { get; set; }

        public string ImageUrl { get
            {
                if (Image != null)
                {
                    return ConfigurationManager.AppSettings["ImagePrefix"] + ConfigurationManager.AppSettings["AzureFolder"] + @"/" + Image;
                }
                else
                {
                    return Image;
                }
            }
        }

        public virtual List<AppUserPermission> AppUserPermissions { get; set; }
    }

    public class AppUserPermission
    {
        public int Id { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Convention Convention { get; set; }
        public UserRole UserRole { get; set; }
    }

    public enum UserRole
    {
        User,
        Editor,
        Admin
    }
}