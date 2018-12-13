using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplayFXSchedule.Web.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Auth0 { get; set; }
        public bool isSuperAdmin { get; set; }

        public virtual List<AppUserPermission> AppUserPermissions { get; set; }
    }

    public class AppUserPermission
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public ReplayConvention Convention { get; set; }
        public UserRole UserRole { get; set; }
    }

    public enum UserRole
    {
        User,
        Editor,
        Admin
    }
}