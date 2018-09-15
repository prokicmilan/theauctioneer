using System.Linq;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class BidRepository : EditableRepositoryBase<Bid>
    {

        public Bid GetTopBidForAuction(int auctionId)
        {
            var query = from bid in context.Bids
                        where bid.AuctionId == auctionId
                        select bid;
            return query.OrderByDescending(bid => bid.BidAmount).FirstOrDefault();
        }

    }
}
