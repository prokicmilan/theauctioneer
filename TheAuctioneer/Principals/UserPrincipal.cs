using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Principals
{
    public class UserPrincipal : IPrincipal
    {
        private readonly UserSessionModel _sessionModel;

        public IIdentity Identity { get; }

        public UserPrincipal(UserSessionModel sessionModel)
        {
            this.Identity = new GenericIdentity(sessionModel.Username);
            this._sessionModel = sessionModel;
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(',');
            return roles.Any(r => r.Equals(_sessionModel.Role));
        }

        public int Id()
        {
            return _sessionModel.Id;
        }

        public string Username()
        {
            return _sessionModel.Username;
        }
    }
}