using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.User;

namespace TheAuctioneer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly AccountBl _accountBl = new AccountBl();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var sessionModel = _accountBl.CreateSessionModel(new LoginUserModel
                    {
                        Username = authTicket.Name,
                        Password = ""
                    });
                    HttpContext.Current.User = new UserPrincipal(sessionModel);
                }
            }
        }
    }
}
