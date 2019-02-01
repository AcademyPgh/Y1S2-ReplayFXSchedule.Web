using ReplayFXSchedule.Web.Models;
using ReplayFXSchedule.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ReplayFXSchedule.Web.Controllers.API
{
    [RoutePrefix("api/v2/users")]
    public class UsersV2Controller : ApiController
    {
        private ReplayFXDbContext db = new ReplayFXDbContext();
        private UserService us;

        [Authorize]
        [Route("")]
        public AppUser GetMe()
        {
            us = new UserService((ClaimsIdentity)User.Identity, db);
            return us.GetUser();
        }

        [Route("{id}")]
        public AppUserView Get(int id)
        {
            var userView = new AppUserView();
            var user = db.AppUsers.Find(id);
            if (user != null)
            {
                userView.DisplayName = user.DisplayName;
                userView.Id = id;
                userView.Image = user.Image;
                userView.ImageUrl = user.ImageUrl;
            }

            return userView;
        }

        // PUT: api/UsersV2/5
        [Route("{id}")]
        [HttpPut]
        [Authorize]
        public void Put(int id, [FromBody]string name, string displayName)
        {
            us = new UserService((ClaimsIdentity)User.Identity, db);
            var user = us.GetUser();
            if (user.Id == id)
            {
                user.Name = name;
                user.DisplayName = displayName;
            }
            db.SaveChanges();
        }


    }
}
