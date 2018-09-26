using BusinessLogicLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace TheAuctioneer.Controllers
{
    public class PaymentController : ApiController
    {

        private readonly TokenOrderBl _tokenOrderBl = new TokenOrderBl();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Payment/Result
        public HttpResponseMessage Get(string reference, string status)
        {
            logger.Debug("Got a request with parameters: status = " + status + ", reference = " + reference);
            var result = _tokenOrderBl.ProcessPayment(Guid.Parse(reference), status);
            if (result == -1)
            {
                // nesto od parametara nije validno, saljemo 406 da bi Centili oznacio transakciju kao odbijenu
                logger.Debug("Invalid parameters, request not acceptable.");
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }
            if (result == -2)
            {
                // desilo se nesto nepredvidjeno, prema dokumentaciji saljemo kod koji nije 200 ili 406, da bi Centili ponovo poslao odgovor
                logger.Debug("Unexpected error, waiting for another request.");
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
            // sve ok, saljemo 200
            logger.Debug("Payment completed.");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
