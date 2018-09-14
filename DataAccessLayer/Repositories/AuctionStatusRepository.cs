using System.Linq;
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
