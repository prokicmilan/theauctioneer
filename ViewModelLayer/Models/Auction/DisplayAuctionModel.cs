using System;
using System.Collections.Generic;
using System.ComponentModel;
using ViewModelLayer.Models.Bid;

namespace ViewModelLayer.Models.Auction
{
    public class DisplayAuctionModel : BaseAuctionModel
    {

        public string Image { get; set; }

        [DisplayName("Highest bidder")]
        public string HighestBidder { get; set; }

        [DisplayName("Expires in")]
        public int H { get; set; }

        public int M { get; set; }

        public int S { get; set; }

        public DateTime ExpiresAt { get; set; }

        public List<DisplayBidModel> bids;
    }
}
