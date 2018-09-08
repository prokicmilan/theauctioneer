using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserBl _userBl = new UserBl();
        // GET: Users
        public ActionResult Index()
        {
            var models = _userBl.DisplayUsers();
            return View(models);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            var model = _userBl.DisplayUserDetails(id);
            return View(model);
        }

        // GET: Users/Register
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
                    int retVal = _userBl.CreateUser(model);
                    switch (retVal)
                    {
                        case 0:
                            return RedirectToAction("Index");
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

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userBl.CheckCredentials(model.Username, model.Password))
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.ErrorMessage = "Credentials invalid.";
                    return View();
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeUserModel model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
