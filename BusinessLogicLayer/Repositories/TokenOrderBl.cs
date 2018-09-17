using DataAccessLayer.Classes;
using DataAccessLayer.Repositories;
using System;

namespace BusinessLogicLayer.Repositories
{
    public class TokenOrderBl
    {

        private readonly TokenOrderRepository _tokenOrderRepository = new TokenOrderRepository();

        private readonly TokenOrderStatusRepository _tokenOrderStatusRepository = new TokenOrderStatusRepository();

        private readonly SystemParameterRepository _systemParameterRepository = new SystemParameterRepository();

        public Guid CreateOrder(Guid userId, string type)
        {
            int amount = 0;
            switch (type)
            {
                case "silver":
                    amount = Convert.ToInt32(_systemParameterRepository.GetByParameterName("S").ParameterValue);
                    break;
                case "gold":
                    amount = Convert.ToInt32(_systemParameterRepository.GetByParameterName("G").ParameterValue);
                    break;
                case "platinum":
                    amount = Convert.ToInt32(_systemParameterRepository.GetByParameterName("P").ParameterValue);
                    break;
            }
            var price = amount * 50;
            var orderId = Guid.NewGuid();
            TokenOrder order = new TokenOrder
            {
                Id = orderId,
                Amount = amount,
                Price = price,
                UserId = userId,
                StatusId = _tokenOrderStatusRepository.GetByType("SUBMITTED").Id
            };
            _tokenOrderRepository.Save(order);
            return orderId;
        }

    }
}
