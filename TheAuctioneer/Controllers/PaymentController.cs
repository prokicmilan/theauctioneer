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

        // GET: Payment/Result
        public HttpResponseMessage CentiliResponse(string userId, string status, string reference)
        {
            var result = _tokenOrderBl.ProcessPayment(Guid.Parse(userId), Guid.Parse(reference), status);
            if (result == -1)
            {
                // nesto od parametara nije validno, saljemo 406 da bi Centili oznacio transakciju kao odbijenu
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }
            if (result == -2)
            {
                // desilo se nesto nepredvidjeno, prema dokumentaciji saljemo kod koji nije 200 ili 406, da bi Centili ponovo poslao odgovor
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
            // sve ok, saljemo 200
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
