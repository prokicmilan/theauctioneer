using DataAccessLayer.Classes;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Repositories
{
    public class TokenOrderBl
    {

        private readonly TokenOrderRepository _tokenOrderRepository = new TokenOrderRepository();

        private readonly TokenOrderStatusRepository _tokenOrderStatusRepository = new TokenOrderStatusRepository();

        public int CreateOrder(int userId, string type)
        {
            int amount = 0;
            switch (type)
            {
                case "silver":
                    amount = 30;
                    break;
                case "gold":
                    amount = 70;
                    break;
                case "platinum":
                    amount = 180;
                    break;
            }
            var price = amount * 50;
            TokenOrder order = new TokenOrder
            {
                Amount = amount,
                Price = price,
                UserId = userId,
                StatusId = _tokenOrderStatusRepository.GetByType("SUBMITTED").Id
            };
            _tokenOrderRepository.Save(order);
            return 0;
        }

    }
}
