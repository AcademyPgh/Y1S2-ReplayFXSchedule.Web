using ReplayFXSchedule.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ReplayFXSchedule.Web.Shared
{
    public class UserService
    {
        private ReplayFXDbContext db;
        private AppUser user;

        public UserService(ClaimsIdentity claimsIdentity, ReplayFXDbContext context)
        {
            db = context;

            var email = claimsIdentity.Claims.Where(c => c.Type.Contains("email")).FirstOrDefault().Value;
            var auth0 = claimsIdentity.Claims.Where(c => c.Type.Contains("nameidentifier")).FirstOrDefault().Value;
            var name = claimsIdentity.Claims.Where(c => c.Type.Contains("nickname")).FirstOrDefault().Value;

            user = db.AppUsers.Where(u => u.Email == email).FirstOrDefault();
            if(user == null)
            {
                user = new AppUser();
                user.Email = email;
                user.Auth0 = auth0;
                user.DisplayName = name;

                db.AppUsers.Add(user);
                db.SaveChanges();
            }
            if (!db.AppUsers.Any(u => u.isSuperAdmin == true))
            {
                user.isSuperAdmin = true;
                db.SaveChanges();
            }

        }

        public AppUser GetUser()
        {
            return user;
        }

        public bool IsConventionAdmin(int convention_id)
        {
            return user.AppUserPermissions.Any(x => x.Convention.Id == convention_id && x.UserRole == UserRole.Admin);
        }

    }
}