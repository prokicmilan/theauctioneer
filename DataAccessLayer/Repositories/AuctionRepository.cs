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
            return _context.Auction.Where(auction => auction.StatusId == 1);
        }

        public IQueryable<Auction> GetAllStarted()
        {
            return _context.Auction.Where(auction => auction.StatusId == 2);
        }

    }
}
