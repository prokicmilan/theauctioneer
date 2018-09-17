using DataAccessLayer.Classes;
using DataAccessLayer.Repositories;
using System;

namespace BusinessLogicLayer.Repositories
{
    public class TokenOrderBl
    {

        private readonly TokenOrderRepository _tokenOrderRepository = new TokenOrderRepository();

        private readonly TokenOrderStatusRepository _tokenOrderStatusRepository = new TokenOrderStatusRepository();

        public int CreateOrder(Guid userId, string type)
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
                Id = Guid.NewGuid(),
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
