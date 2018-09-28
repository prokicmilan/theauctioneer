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

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Login
        [Route]
        public ActionResult Index(string returnUrl = "")
        {
            logger.Info("returnUrl = " + returnUrl);
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                logger.Info("User not authenticated, redirecting to login page.");
                if (returnUrl.Length != 0)
                {
                    TempData["ReturnUrl"] = returnUrl;
                }
                return View("Login");
            }

            return RedirectToAction("Index", "Auctions");
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
                        logger.Info("Valid credentials, logging in.");
                        var sessionModel = _accountBl.CreateSessionModel(model);
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
                        return RedirectToAction("Index", "Auctions");
                    }
                    logger.Info("Invalid credentials for user " + model.Username);
                    ViewBag.ErrorMessage = "Invalid credentials.";
                    return View("Login");
                }
                logger.Info("Invalid model state.");
                return View("Login");
            }
            catch (Exception e)
            {
                logger.Error("Exception occured, redirecting to login." + e.Message);
                return View("Login");
            }
        }

        public ActionResult Register()
        {
            logger.Info("");
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
                            logger.Info("User created. Username = " + model.Username);
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
                            return RedirectToAction("Index", "Auctions");
                        case -1:
                            logger.Info("Username " + model.Username + " already exists.");
                            ModelState.AddModelError("Username", "Specified username is already taken.");
                            return View();
                        case -2:
                            logger.Info("Email " + model.Email + " already exists.");
                            ModelState.AddModelError("Email", "Specified email address already exists in the system.");
                            return View();
                    }
                }
                logger.Info("Invalid model state.");
                return View();
            }
            catch (Exception e)
            {
                logger.Error("Exception occured, redirecting to registration page." + e.Message);
                return View();
            }
        }

        // GET: /Account/Details/5
        public ActionResult Details(Guid id)
        {
            logger.Info("id = " + id);
            var model = _accountBl.DisplayUserDetails(id);
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult LogOff()
        {
            logger.Info(((UserPrincipal)HttpContext.User).Username);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        // GET: /Account/Edit/5
        [AuthorizeSelf]
        public ActionResult Edit(Guid id)
        {
            logger.Info("id = " + id);
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
                logger.Info("Model state invalid");
                return View();
            }
            catch (Exception e)
            {
                logger.Error("Exception occured, redirecting to edit page. " + e.Message);
                return View();
            }
        }

        [AuthorizeSelf]
        // GET: Account/ChangePassword/5
        public ActionResult ChangePassword(Guid id)
        {
            logger.Info("id = " + id);
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
                        logger.Info("Password changed id = " + model.Id);
                        return RedirectToAction("Index", "Users");
                    }
                    logger.Info("Password invalid, id = " + model.Id);
                    ModelState.AddModelError("OldPassword", "Password is invalid.");
                    return View();
                }
                logger.Info("Model state invalid.");
                return View();
            }
            catch (Exception e)
            {
                logger.Error("Exception occured, redirecting to password change page. " + e.Message);
                return View();
            }
        }

        // GET: /Account/Unauthorized
        public ActionResult Unauthorized()
        {
            TempData["ErrorMessage"] = "You are not authorized to perform that action.";
            return RedirectToAction("Index", "Auctions");
        }

    }
}