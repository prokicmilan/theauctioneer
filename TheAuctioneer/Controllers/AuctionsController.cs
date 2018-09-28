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
            logger.Info("page = " + page
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
            logger.Info("");
            var models = _auctionBl.GetAllCreatedByUser(((UserPrincipal) HttpContext.User).Id);
            return View(models);
        }

        // GET: Auctions/Create
        public ActionResult Create()
        {
            logger.Info("");
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
                logger.Info("Model state invalid.");
                return View();
            }
            catch (Exception e) 
            {
                logger.Info("Exception occured, redirecting to create auction page. " + e.Message);
                return View();
            }
        }

        // GET: Auctions/ShowReady
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult ShowReady()
        {
            logger.Info("");
            var models = _auctionBl.GetAllReady();
            return View(models);
        }

        // GET: Auctions/Details/5
        public ActionResult Details(Guid id)
        {
            logger.Info("id = " + id);
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // GET: Auctions/Delete/5
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Delete(Guid id)
        {
            logger.Info("id = " + id);
            var model = _auctionBl.DisplayAuctionDetails(id);
            return View(model);
        }

        // POST: Auctions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(RolesAllowed = "Admin")]
        public ActionResult Delete(DisplayAuctionModel model)
        {
            logger.Info("Deleting auction " + model.Id);
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
            logger.Info("Starting auction " + model.Id);
            _auctionBl.StartAuction(model);
            return RedirectToAction("ShowReady");
        }

        [HttpPost]
        public ActionResult Bid(Guid id)
        {
            logger.Info("id = " + id);
            var retVal = _auctionBl.PostBid(id, ((UserPrincipal) HttpContext.User).Id);
            switch (retVal)
            {
                case 0:
                    logger.Info("Bid successful");
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<BidHub>();
                    var price = _auctionBl.GetAuctionPrice(id);
                    hubContext.Clients.All.UpdateAuction(id, price, ((UserPrincipal) HttpContext.User).Username);
                    return Redirect(Request.UrlReferrer.ToString());
                case -1:
                    logger.Info("Auction expired.");
                    TempData["ErrorMessage"] = "The auction has already expired.";
                    break;
                case -2:
                    logger.Info("User is the owner.");
                    TempData["ErrorMessage"] = "You are the owner of the auction.";
                    break;
                case -3:
                    logger.Info("User is the highest bidder.");
                    TempData["ErrorMessage"] = "You're already the highest bidder.";
                    break;
                case -4:
                    logger.Info("User doesn't have enough tokens.");
                    TempData["ErrorMessage"] = "You don't have enough tokens to make that bid.";
                    break;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListWon()
        {
            logger.Info("");
            var models = _auctionBl.GetAllWonByUser(((UserPrincipal) HttpContext.User).Id);
            return View("ShowReady", models);
        }
    }
}