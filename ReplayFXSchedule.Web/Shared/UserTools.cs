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

                if (!db.AppUsers.Any(u => u.isSuperAdmin == true))
                {
                    user.isSuperAdmin = true;
                }

                db.AppUsers.Add(user);
                db.SaveChanges();
            }
            
        }
        //        private ReplayFXDbContext _context;
        //        private AppUser _user;

        //        public UserService(ClaimsIdentity claimsIdentity, ReplayFXDbContext context)
        //        {
        //            _context = context;

        //            var email = claimsIdentity.Claims.Where(c => c.Type.Contains("emailaddress")).FirstOrDefault().Value;
        //            var auth0 = claimsIdentity.Claims.Where(c => c.Type.Contains("nameidentifier")).FirstOrDefault().Value;
        //            var name = claimsIdentity.Claims.Where(c => c.Type.Contains("name")).FirstOrDefault().Value;
        //            var user = _context.AppUsers
        //                .Include(u => u.UserRoles)
        //                    .ThenInclude(x => x.Customer)
        //                        .ThenInclude(y => y.Locations)
        //                            .ThenInclude(z => z.LocationPics)
        //                .Include(u => u.UserRoles)
        //                    .ThenInclude(x => x.Customer)
        //                        .ThenInclude(y => y.Events)
        //                .Where(u => u.Email == email).FirstOrDefault();
        //            if (user == null)
        //            {
        //                user = new User();
        //                user.Email = email;
        //                user.Auth0Id = auth0;
        //                user.Name = name;
        //                context.Users.Add(user);
        //                context.SaveChanges();
        //                // super basic
        //            }
        //            if (user.Auth0Id != auth0)
        //            {
        //                user.Auth0Id = auth0;
        //                context.SaveChanges();
        //            }
        //            if (!_context.UserRoles.Any(x => x.UserType == UserType.Administrator)) // there are no admins in the system, make this user an admin
        //                                                                                    // a better route is to have a whitelist of emails in config
        //            {
        //                var newRole = new UserRole();
        //                newRole.UserType = UserType.Administrator;
        //                newRole.User = user;
        //                _context.UserRoles.Add(newRole);
        //                _context.SaveChanges();
        //            }

        //            _user = user;
        //        }

        //        public AppUser GetUser()
        //        {
        //            return _user;
        //        }

        //        public bool IsAdmin
        //        {
        //            get
        //            {
        //                return _user.UserRoles.Any(x => x.UserType == UserType.Administrator);
        //            }
        //        }

    }
}