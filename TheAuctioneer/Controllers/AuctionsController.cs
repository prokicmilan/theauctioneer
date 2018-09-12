using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer.Repositories;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;
using ViewModelLayer.Models.Auction;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class AuctionsController : Controller
    {
        private readonly AuctionBl _auctionBl = new AuctionBl();

        // GET: Auctions
        [AllowAnonymous]
        public ActionResult Index()
        {
            var models = _auctionBl.GetAllStarted();
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
            _auctionBl.IncreasePrice(id, ((UserPrincipal) HttpContext.User).Id());
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ListWon()
        {
            var models = _auctionBl.GetAllWonByUser(((UserPrincipal) HttpContext.User).Id());
            return View("ShowReady", models);
        }
    }
}