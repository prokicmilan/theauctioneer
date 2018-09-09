using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Controllers
{
    [RoutePrefix("Login")]
    public class AccountController : Controller
    {

        private readonly AccountBl _accountBl = new AccountBl();

        // GET: Login
        [Route]
        public ActionResult Index(string returnUrl = "")
        {
            if (Session["UserSession"] == null)
            {
                if (returnUrl.Length != 0)
                {
                    TempData["ReturnUrl"] = returnUrl;
                }
                return View("Login");
            }

            return RedirectToAction("Index", "Users");
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public ActionResult Index(LoginUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_accountBl.CheckCredentials(model.Username, model.Password))
                    {
                        var sessionModel = _accountBl.CreateSessionModel(model);
                        Session["UserSession"] = sessionModel;
                        // TODO: cookie?
                        var authTicket = new FormsAuthenticationTicket(
                                1,
                                sessionModel.Id.ToString(),
                                DateTime.Now,
                                DateTime.Now.AddMinutes(1),
                                model.RememberMe,
                                sessionModel.Role,
                                "/"
                            );
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        Response.Cookies.Add(cookie);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"] as string);
                        }
                        return RedirectToAction("Index", "Users");
                    }

                    ViewBag.ErrorMessage = "Invalid credentials.";
                    return View("Login");
                }

                return View("Login");
            }
            catch
            {
                return View("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CreateUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int retVal = _accountBl.CreateUser(model);
                    switch (retVal)
                    {
                        case 0:
                            var sessionUser = _accountBl.CreateSessionModel(new LoginUserModel
                            {
                                Username = model.Username,
                                Password = ""
                            }
                            );
                            Session["UserSession"] = sessionUser;
                            return RedirectToAction("Index", "Users");
                        case -1:
                            ModelState.AddModelError("Username", "Specified username is already taken.");
                            return View();
                        case -2:
                            ModelState.AddModelError("Email", "Specified email address already exists in the system.");
                            return View();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult LogOff()
        {
            Session["UserSession"] = null;

            //TODO: invalidacija cookiea?
            return RedirectToAction("Index");
        }

        // GET: /Account/Unauthorized
        public ActionResult Unauthorized()
        {
            TempData["ErrorMessage"] = "You are not authorized to perform that action.";
            return RedirectToAction("Index", "Users");
        }

    }
}