using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using Microsoft.AspNet.SignalR;
using TheAuctioneer.Attributes;
using TheAuctioneer.Hubs;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.Auction;
using X.PagedList;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class AuctionsController : Controller
    {
        private readonly AuctionBl _auctionBl = new AuctionBl();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Auctions
        [AllowAnonymous]
        public ActionResult Index(int? page, int? priceLow, int? priceHigh, int? itemsPerPage, string sortingOrder, string searchString)
        {
            logger.Debug("Index() --> page = " + page
                                    + "priceLow = " + priceLow
                                    + "priceHigh = " + priceHigh
                                    + "sortingOrder = " + sortingOrder
                                    + "searchString = " + searchString);
            var pageNumber = page ?? 1;
            // po defaultu sortiramo rastuce prema preostalom vremenu (prvo najskorije)
            bool descending = false;
            if (!String.IsNullOrEmpty(sortingOrder) && sortingOrder.Equals("Descending"))
            {
                descending = true;
            }
            var models = _auctionBl.GetAllStarted(priceLow, priceHigh, searchString);
            if (descending)
            {
                models.Sort((x, y) => y.ExpiresAt.CompareTo(x.ExpiresAt));
            }
            else
            {
                models.Sort((x, y) => x.ExpiresAt.CompareTo(y.ExpiresAt));
            }
            if (itemsPerPage == null)
            {
                itemsPerPage = 10;
            }
            var pagedModels = models.ToPagedList(pageNumber, (int) itemsPerPage);
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(pagedModels);
        }

        // GET: Auctions/My
        public ActionResult My()
        {
            var models = _auctionBl.GetAllCreatedByUser(((UserPrincipal) HttpContext.User).Id);
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
                    var userId = ((UserPrincipal)(HttpContext.User)).Id;
                    _auctionBl.CreateAuction(model, userId);
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
        public ActionResult Details(Guid id)
        {
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // GET: Auctions/Delete/5
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Delete(Guid id)
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
        public ActionResult Start(Guid id)
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
        public ActionResult Bid(Guid id)
        {
            var retVal = _auctionBl.PostBid(id, ((UserPrincipal) HttpContext.User).Id);
            switch (retVal)
            {
                case 0:
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<BidHub>();
                    var price = _auctionBl.GetAuctionPrice(id);
                    hubContext.Clients.All.UpdateAuction(id, price, ((UserPrincipal) HttpContext.User).Username);
                    return Redirect(Request.UrlReferrer.ToString());
                case -1:
                    TempData["ErrorMessage"] = "The auction has already expired.";
                    break;
                case -2:
                    TempData["ErrorMessage"] = "You are the owner of the auction.";
                    break;
                case -3:
                    TempData["ErrorMessage"] = "You're already the highest bidder.";
                    break;
                case -4:
                    TempData["ErrorMessage"] = "You don't have enough tokens to make that bid.";
                    break;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListWon()
        {
            var models = _auctionBl.GetAllWonByUser(((UserPrincipal) HttpContext.User).Id);
            return View("ShowReady", models);
        }
    }
}