using DataAccessLayer.Classes;
using DataAccessLayer.Repositories;
using System;
using System.Transactions;

namespace BusinessLogicLayer.Repositories
{
    public class TokenOrderBl
    {

        private readonly TokenOrderRepository _tokenOrderRepository = new TokenOrderRepository();

        private readonly TokenOrderStatusRepository _tokenOrderStatusRepository = new TokenOrderStatusRepository();

        private readonly UserRepository _userRepository = new UserRepository();

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

        public int ProcessPayment(Guid userId, Guid orderId, string status)
        {
            using (var tx = new TransactionScope())
            {
                try
                {
                    var order = _tokenOrderRepository.GetById(orderId);
                    // nema narudzbine
                    if (order == null)
                    {
                        tx.Dispose();
                        return -1;
                    }
                    // narudzbina je za drugog korisnika
                    if (order.UserId != userId)
                    {
                        tx.Dispose();
                        return -1;
                    }
                    // narudzbina je vec obradjena
                    if (order.StatusId != _tokenOrderStatusRepository.GetByType("SUBMITTED").Id)
                    {
                        tx.Dispose();
                        return -1;
                    }
                    // placanje neuspesno na Centili strani
                    if (status.Equals("canceled") || status.Equals("failed"))
                    {
                        order.StatusId = _tokenOrderStatusRepository.GetByType("CANCELED").Id;
                        _tokenOrderRepository.Save(order);
                        tx.Complete();
                        return 0;
                    }
                    // sve ok, dodajemo korisniku tokene
                    order.StatusId = _tokenOrderStatusRepository.GetByType("COMPLETED").Id;
                    var user = _userRepository.GetById(userId);
                    user.TokenCount += order.Amount;
                    _tokenOrderRepository.Save(order);
                    _userRepository.Save(user);
                    tx.Complete();
                    return 0;
                }
                catch
                {
                    tx.Dispose();
                    return -2;
                }
            }
        }

    }
}
