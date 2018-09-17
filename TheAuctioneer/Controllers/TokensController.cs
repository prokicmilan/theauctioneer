using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class TokensController : Controller
    {
        // GET: Tokens/Buy
        public ActionResult Buy()
        {
            return View();
        }
        
        // POST: Tokens/Buy
        [HttpPost]
        public ActionResult Buy(string type)
        {
            // create the order
            var userId = ((UserPrincipal)HttpContext.User).Id();
            var orderId = 20;
            return Redirect("http://stage.centili.com/payment/widget?apikey=5cdc11a42057fb2bc55b6ab4e9801917&country=rs&userId=" + userId + "&reference=" + orderId);
        }

    }
}