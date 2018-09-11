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

    }
}
