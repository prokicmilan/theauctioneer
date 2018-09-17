using System;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;
using X.PagedList;

namespace TheAuctioneer.Controllers
{
    public class UsersController : Controller
    {
        private readonly AccountBl _accountBl = new AccountBl();

        // GET: Users

        public ActionResult Index(int? page)
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var pageNumber = page ?? 1;
            var models = _accountBl.DisplayUsers().ToPagedList(pageNumber, 2);
            return View(models);
        }

        // GET: Users/Details/5
        [AuthorizeSelf]
        public ActionResult Details(Guid id)
        {
            if (((UserPrincipal)HttpContext.User).Id != id)
            {
                return RedirectToAction("Unauthorized", "Account");
            }
            var model = _accountBl.DisplayUserDetails(id);
            return View(model);
        }


    }
}
