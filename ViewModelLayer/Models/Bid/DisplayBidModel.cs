using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.Bid
{
    public class DisplayBidModel
    {

        public string Username { get; set; }

        [DisplayName("Posted at")]
        public DateTime Timestamp { get; set; }

        public int BidAmount { get; set; }

    }
}
