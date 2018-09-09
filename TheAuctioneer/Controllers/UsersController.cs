using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.User;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class UsersController : Controller
    {
        private readonly AccountBl _userBl = new AccountBl();

        // GET: Users
        public ActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var models = _userBl.DisplayUsers();
            return View(models);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            if (((UserPrincipal)HttpContext.User).Id() != id)
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            var model = _userBl.DisplayUserDetails(id);
            return View(model);
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
