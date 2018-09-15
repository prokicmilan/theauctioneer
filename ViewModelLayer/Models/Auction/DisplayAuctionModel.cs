using System.ComponentModel;

namespace ViewModelLayer.Models.Auction
{
    public class DisplayAuctionModel : BaseAuctionModel
    {

        public string Image { get; set; }

        [DisplayName("Expires in")]
        public int H { get; set; }

        public int M { get; set; }

        public int S { get; set; }

    }
}
