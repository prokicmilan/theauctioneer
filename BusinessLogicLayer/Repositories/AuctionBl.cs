using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using DataAccessLayer.Classes;
using DataAccessLayer.Repositories;
using ViewModelLayer.Models.Auction;

namespace BusinessLogicLayer.Repositories
{
    public class AuctionBl
    {

        private readonly AuctionRepository _auctionRepository = new AuctionRepository();

        private readonly AuctionStatusRepository _auctionStatusRepository = new AuctionStatusRepository();
    
        private readonly BidRepository _bidRepository = new BidRepository();

        private readonly UserRepository _userRepository = new UserRepository();

        public List<DisplayAuctionModel> GetAllReady()
        {
            var auctions = _auctionRepository.GetAllReady().ToList();
            var models = new List<DisplayAuctionModel>();
            foreach (var auction in auctions)
            {
                var model = InitDisplayAuctionModel(auction);
                models.Add(model);
            }

            return models;
        }

        public List<DisplayAuctionModel> GetAllStarted()
        {
            MarkExpiredAsCompleted();
            var auctions = _auctionRepository.GetAllStarted();
            var models = new List<DisplayAuctionModel>();
            foreach (var auction in auctions)
            {
                var model = InitDisplayAuctionModel(auction);
                models.Add(model);
            }

            return models;
        }

        public void CreateAuction(CreateAuctionModel model)
        {
            var auction = new Auction
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Duration = model.Duration
            };
            var target = new MemoryStream();
            model.Image.InputStream.CopyTo(target);
            var data = target.ToArray();
            auction.Image = data;
            auction.StatusId = _auctionStatusRepository.GetByType("READY").Id;
            _auctionRepository.Save(auction);
        }

        public DisplayAuctionModel DisplayAuctionDetails(int id)
        {
            return InitDisplayAuctionModel(_auctionRepository.GetById(id));
        }

        public void DeleteAuction(DisplayAuctionModel model)
        {
            var auction = _auctionRepository.GetById(model.Id);
            _auctionRepository.Remove(auction);
        }

        public void StartAuction(DisplayAuctionModel model)
        {
            MarkExpiredAsCompleted();
            var auction = _auctionRepository.GetById(model.Id);
            auction.ExpiresAt = DateTime.Now.AddSeconds(auction.Duration);
            auction.StatusId = _auctionStatusRepository.GetByType("OPENED").Id;
            _auctionRepository.Save(auction);
        }

        public int PostBid(int auctionId, int userId)
        {
            using (var tx = new TransactionScope())
            {
                MarkExpiredAsCompleted();
                var auction = _auctionRepository.GetById(auctionId);
                if (auction.ExpiresAt <= DateTime.Now)
                {
                    tx.Complete();
                    return -1;
                }
                var user = _userRepository.GetById(userId);
                if ((user.TokenCount - (auction.Price)) < 0)
                {
                    tx.Dispose();
                    return -2;
                }
                var oldBid = _bidRepository.GetTopBidForAuction(auctionId);
                if (oldBid != null && oldBid.UserId == userId)
                {
                    tx.Dispose();
                    return -3;
                }
                if (oldBid != null)
                {
                    var oldUser = _userRepository.GetById(oldBid.UserId);
                    oldUser.TokenCount += oldBid.BidAmount;
                    _userRepository.Save(oldUser);
                }

                auction.Price++;
                var bid = new Bid
                {
                    UserId = userId,
                    AuctionId = auctionId,
                    Timestamp = DateTime.Now,
                    BidAmount = auction.Price
                };
                _bidRepository.Save(bid);
                _auctionRepository.Save(auction);
                user.TokenCount -= auction.Price;
                _userRepository.Save(user);
                tx.Complete();
                return 0;
            }
        }

        public List<DisplayAuctionModel> GetAllWonByUser(int userId)
        {
            var auctions = _auctionRepository.GetAllWonByUser(userId);
            var models = new List<DisplayAuctionModel>();
            foreach (var auction in auctions)
            {
                var model = InitDisplayAuctionModel(auction);
                models.Add(model);
            }

            return models;
        }

        private DisplayAuctionModel InitDisplayAuctionModel(Auction auction)
        {
            var model = new DisplayAuctionModel
            {
                Id = auction.Id,
                Name = auction.Name,
                Description = auction.Description,
                Price = auction.Price,
                Image = "data:image/png;base64," + Convert.ToBase64String(auction.Image, 0, auction.Image.Length)
            };
            var expiresAt = auction.ExpiresAt != DateTime.MinValue ? auction.ExpiresAt : DateTime.MaxValue;
            var timeLeft = expiresAt - DateTime.Now;
            model.H = timeLeft.Hours;
            model.M = timeLeft.Minutes;
            model.S = timeLeft.Seconds;

            return model;
        }

        private void MarkExpiredAsCompleted()
        {
            IList<Auction> auctions = _auctionRepository.GetAllStarted().ToList();
            var completedId = _auctionStatusRepository.GetByType("COMPLETED").Id;
            foreach (var auction in auctions)
            {
                if (auction.ExpiresAt < DateTime.Now)
                {
                    auction.StatusId = completedId;
                    _auctionRepository.Save(auction);
                }
            }
        }

    }
}
