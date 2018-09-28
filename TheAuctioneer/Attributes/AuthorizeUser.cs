using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Attributes
{
    internal class AuthorizeUser : AuthorizeAttribute
    {
        private readonly AccountBl _accountBl = new AccountBl();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string RolesAllowed { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessionUser = httpContext.User;
            if (!sessionUser.Identity.IsAuthenticated)
            {
                return false;
            }

            if (RolesAllowed != null)
            {
                return sessionUser.IsInRole(RolesAllowed);
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                logger.Info("Unauthorized acces by authenticated user, username = " + ((UserPrincipal)(filterContext.HttpContext.User)).Username);
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
                logger.Info("Unauthorized access by anonymous user.");
                var returl = "";
                if (filterContext.HttpContext.Request.HttpMethod.Equals("GET"))
                {
                    returl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
                }
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Login",
                                action = "Index",
                                returnUrl = returl
                            }
                        )
                    );
            }
        }
    }
}