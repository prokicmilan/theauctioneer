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

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Users/Details/5
        [AuthorizeSelf]
        public ActionResult Details(Guid id)
        {
            logger.Info("id = " + id);
            var model = _accountBl.DisplayUserDetails(id);
            return View(model);
        }


    }
}
