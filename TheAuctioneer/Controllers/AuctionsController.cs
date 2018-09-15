using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.Auction;
using X.PagedList;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class AuctionsController : Controller
    {
        private readonly AuctionBl _auctionBl = new AuctionBl();

        // GET: Auctions
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var models = _auctionBl.GetAllStarted().ToPagedList(pageNumber, 10);
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(models);
        }

        // GET: Auctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAuctionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _auctionBl.CreateAuction(model);
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Auctions/ShowReady
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult ShowReady()
        {
            var models = _auctionBl.GetAllReady();
            return View(models);
        }

        // GET: Auctions/Details/5
        public ActionResult Details(int id)
        {
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // GET: Auctions/Delete/5
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Delete(int id)
        {
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // POST: Auctions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Delete(DisplayAuctionModel model)
        {
            _auctionBl.DeleteAuction(model);
            return RedirectToAction("ShowReady");
        }

        // GET: Auctions/Start/5
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Start(int id)
        {
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // POST: Auctions/Start/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Start(DisplayAuctionModel model)
        {
            _auctionBl.StartAuction(model);
            return RedirectToAction("ShowReady");
        }

        [HttpPost]
        public ActionResult Bid(int id)
        {
            var retVal = _auctionBl.PostBid(id, ((UserPrincipal) HttpContext.User).Id());
            switch (retVal)
            {
                case 0:
                    return Redirect(Request.UrlReferrer.ToString());
                case -1:
                    TempData["ErrorMessage"] = "The auction has already expired.";
                    break;
                case -2:
                    TempData["ErrorMessage"] = "You don't have enough tokens to make that bid.";
                    break;
                case -3:
                    TempData["ErrorMessage"] = "You're already the highest bidder on that auction.";
                    break;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListWon()
        {
            var models = _auctionBl.GetAllWonByUser(((UserPrincipal) HttpContext.User).Id());
            return View("ShowReady", models);
        }
    }
}