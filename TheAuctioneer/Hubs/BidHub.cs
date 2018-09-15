using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TheAuctioneer.Hubs
{
    public class BidHub : Hub
    {
        private static int num = 0;
        public void AlertAll()
        {
            Clients.All.BroadcastMessage("signalR" + num++);
        }
    }
}