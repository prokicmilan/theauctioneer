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
                        where bid.BidAmount == (from b in context.Bids
                                                where b.AuctionId == auctionId
                                                select bid.BidAmount).Max()
                        select bid;
            return query.FirstOrDefault();
        }

    }
}
