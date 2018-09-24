using System;
using System.Linq;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class BidRepository : EditableRepositoryBase<Bid>
    {

        public IQueryable<Bid> GetAllBidsForAuction(Guid auctionId)
        {
            return context.Bids.Where(bid => bid.AuctionId == auctionId);
        }

        public Bid GetTopBidForAuction(Guid auctionId)
        {
            var query = from bid in context.Bids
                        where bid.AuctionId == auctionId
                        select bid;
            return query.OrderByDescending(bid => bid.BidAmount).FirstOrDefault();
        }

    }
}
