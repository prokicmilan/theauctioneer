using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TheAuctioneer.Principals;

namespace TheAuctioneer.Attributes
{
    public class AuthorizeSelf : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            UserPrincipal sessionUser = httpContext.User as UserPrincipal;
            if (sessionUser == null) return false;
            var requestPath = httpContext.Request.Path;
            var substrStart = requestPath.LastIndexOf('/') + 1;
            var substrLen = requestPath.Length - substrStart;
            if (substrLen == 0) substrLen++;
            var requestId = new Guid(requestPath.Substring(substrStart, substrLen));
            // odvratno budzenje ali zivot je tezak, mozda niko ne procita ovo :(
            return sessionUser.Identity.IsAuthenticated && (sessionUser.Id == requestId || sessionUser.IsInRole("Admin"));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
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