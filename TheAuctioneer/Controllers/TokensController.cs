using BusinessLogicLayer.Repositories;
using System.Linq;
using System.Web.Mvc;
using TheAuctioneer.Attributes;
using TheAuctioneer.Principals;

namespace TheAuctioneer.Controllers
{
    [AuthorizeUser]
    public class TokensController : Controller
    {

        private readonly TokenOrderBl _tokenOrderBl = new TokenOrderBl();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Tokens
        public ActionResult Index()
        {
            logger.Info("");
            var model = _tokenOrderBl.GetAllOrdersByUser(((UserPrincipal)HttpContext.User).Id);
            model.Sort((x, y) => y.TimestampCreated.CompareTo(x.TimestampCreated));
            return View("TokenOrders", model);
        }

        // GET: Tokens/Buy
        public ActionResult Buy()
        {
            logger.Info("");
            return View();
        }
        
        // POST: Tokens/Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(string type)
        {
            logger.Info("type = " + type);
            // gadim se samom sebi
            if (!"goldsilverplatinum".Contains(type))
            {
                logger.Error("Invalid type = " + type);
                return View();
            }
            var userId = ((UserPrincipal)HttpContext.User).Id;
            var orderId = _tokenOrderBl.CreateOrder(userId, type);
            return Redirect("http://stage.centili.com/payment/widget?apikey=5cdc11a42057fb2bc55b6ab4e9801917&country=rs&reference=" + orderId + "&returnurl=http://theauctioneer.azurewebsites.net/api/Payment/");
        }

    }
}