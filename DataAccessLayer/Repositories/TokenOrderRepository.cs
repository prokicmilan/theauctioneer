using DataAccessLayer.Classes;
using System;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TokenOrderRepository : EditableRepositoryBase<TokenOrder>
    {

        public IQueryable<TokenOrder> GetAllOrdersByUser(Guid userId)
        {
            return context.TokenOrders.Where(order => order.UserId == userId);
        }

    }
}
