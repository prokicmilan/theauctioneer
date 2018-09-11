using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AuctionStatusRepository : ReadOnlyRepositoryBase<AuctionStatus>
    {
        public AuctionStatus GetByType(string type)
        {
            return _context.AuctionStatus.Single(status => status.Type.Equals(type));
        }
    }
}
