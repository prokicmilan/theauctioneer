using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AuctionRepository : EditableRepositoryBase<Auction>
    { 

        public IQueryable<Auction> GetAllReady()
        {
            /*
             * select
             *      Auction.*
             * from
             *      Auction
             * join
             *      AuctionStatus
             * on
             *      AuctionStatus.Id = Auction.StatusId
             * where
             *      AuctionStatus.[Type] = 'READY'
             */
            return _context.Auction.Join(_context.AuctionStatus,                                                      // tabela za spajanje
                                                auction => auction.StatusId,                                          // strani kljuc
                                                status => status.Id,                                                  // primarni kljuc
                                                (auction, status) => new {Auction = auction, AuctionStatus = status}) // agregacija
                                         .Where(status => status.AuctionStatus.Type.Equals("READY"))                  // where
                                         .Select(auction => auction.Auction);                                         // select
        }

        public IQueryable<Auction> GetAllStarted()
        {
            /*
             * select
             *      Auction.*
             * from
             *      Auction
             * join
             *      AuctionStatus
             * on
             *      AuctionStatus.Id = Auction.StatusId
             * where
             *      AuctionStatus.[Type] = 'OPENED'
             */
            return _context.Auction.Join(_context.AuctionStatus,
                                         auction => auction.StatusId,
                                         status => status.Id,
                                         (auction, status) => new { Auction = auction, AuctionStatus = status })
                                   .Where(status => status.AuctionStatus.Type.Equals("OPENED"))
                                   .Select(auction => auction.Auction);
        }
        /*
         * select
	     *      [a].*
         *  from
	     *      Auction [a]
         *  join
         *      AuctionStatus [as]
         *  on
         *      [as].Id = [a].StatusId
         *  and
         *      [as].Type = 'COMPLETED'
         *  join
	     *      Bid [b]
         *  on
	     *      [b].AuctionId = [a].Id
         *  and
	     *      [b].UserId = @userId
         *  where
	     *      [b].BidAmount = (select
		 *			                max([b2].BidAmount)
		 * 		                 from
		 *      		            Bid [b2]
		 *  		             where
		 *  			            [b2].AuctionId = [b].AuctionId)
         */
        public IQueryable<Auction> GetAllWonByUser(int userId)
        {
            var query = from auction in _context.Auction
                        join status in _context.AuctionStatus 
                        on new { StatusId = auction.StatusId, Type = "COMPLETED" } equals new { StatusId = status.Id, Type = status.Type } 
                        join bid in _context.Bid
                        on new { AuctionId = auction.Id, UserId = userId } equals new { AuctionId = bid.AuctionId, UserId = bid.UserId }
                        where bid.BidAmount == (from bidAggr in _context.Bid
                                                where bidAggr.AuctionId == bid.AuctionId
                                                select bidAggr.BidAmount).Max()
                        select auction;

            return query;
        }
    }
}
