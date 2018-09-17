using DataAccessLayer.Classes;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TokenOrderStatusRepository : ReadOnlyRepositoryBase<TokenOrderStatus>
    {

        public TokenOrderStatus GetByType(string type)
        {
            return context.TokenOrderStatuses.Single(status => status.Type.Equals(type));
        }

    }
}
