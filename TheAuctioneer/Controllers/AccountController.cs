using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;
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
            if (!HttpContext.User.Identity.IsAuthenticated)
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
                        //Session["UserSession"] = sessionModel;
                        HttpContext.User = new UserPrincipal(sessionModel);
                        // TODO: cookie?
                        FormsAuthentication.SetAuthCookie(sessionModel.Username, model.RememberMe);
                        var authTicket = new FormsAuthenticationTicket(
                                1,
                                sessionModel.Username,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(60),
                                model.RememberMe,
                                sessionModel.Role
                            );
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
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
                            HttpContext.User = new UserPrincipal(sessionUser);
                            FormsAuthentication.SetAuthCookie(sessionUser.Username, false);
                            var authTicket = new FormsAuthenticationTicket(
                                1,
                                sessionUser.Username,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(60),
                                false,
                                sessionUser.Role
                            );
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            HttpContext.Response.Cookies.Add(authCookie);
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

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult LogOff()
        {
            //Session["UserSession"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        // GET: /Account/Edit/5
        [AuthorizeSelf]
        public ActionResult Edit(int id)
        {
            var model = _accountBl.DisplayUserDetails(id);
            return View(model);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [AuthorizeSelf]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisplayUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _accountBl.ChangeUserDetails(model);
                    return RedirectToAction("Index", "Users");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [AuthorizeSelf]
        // GET: Account/ChangePassword/5
        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        // POST: Account/ChangePassword/5
        [HttpPost]
        [AuthorizeSelf]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordChangeUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_accountBl.ChangePassword(model))
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    ModelState.AddModelError("OldPassword", "Password is invalid.");
                    return View();
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: /Account/Unauthorized
        public ActionResult Unauthorized()
        {
            TempData["ErrorMessage"] = "You are not authorized to perform that action.";
            return RedirectToAction("Index", "Users");
        }

    }
}