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
    public class UsersController : Controller
    {
        private readonly AccountBl _accountBl = new AccountBl();

        // GET: Users

        public ActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var models = _accountBl.DisplayUsers();
            return View(models);
        }

        // GET: Users/Details/5
        [AuthorizeSelf]
        public ActionResult Details(int id)
        {
            if (((UserPrincipal)HttpContext.User).Id() != id)
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            var model = _accountBl.DisplayUserDetails(id);
            return View(model);
        }


    }
}
