using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLogicLayer.Repositories;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Attributes
{
    internal class AuthorizeUser : AuthorizeAttribute
    {
        private readonly AccountBl _accountBl = new AccountBl();

        public string RolesAllowed { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessionUser = httpContext.Session["UserSession"];
            if (sessionUser == null)
            {
                return false;
            }

            if (RolesAllowed != null)
            {
                return RolesAllowed.Contains(((UserSessionModel) sessionUser).Role) || ((UserSessionModel) sessionUser).Role.Equals("Admin");
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserSession"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Unauthorized"
                            }
                        )
                    );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Login",
                                action = "Index",
                                returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                            }
                        )
                    );
            }
        }
    }
}