using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class AuctionStatusRepository : ReadOnlyRepositoryBase<AuctionStatus>
    {
        public AuctionStatus GetByType(string type)
        {
            return context.AuctionStatuses.Single(status => status.Type.Equals(type));
        }
    }
}
