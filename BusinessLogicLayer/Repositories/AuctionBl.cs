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

        public List<DisplayAuctionModel> GetAllStarted(int? priceLow, int? priceHigh, string searchString)
        {
            MarkExpiredAsCompleted();
            var auctions = _auctionRepository.FilterStarted(priceLow, priceHigh, searchString);
            var models = new List<DisplayAuctionModel>();
            foreach (var auction in auctions)
            {
                var model = InitDisplayAuctionModel(auction);
                models.Add(model);
            }

            return models;
        }

        public void CreateAuction(CreateAuctionModel model, Guid userId)
        {
            var auction = new Auction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
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

        public DisplayAuctionModel DisplayAuctionDetails(Guid id)
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

        public int PostBid(Guid auctionId, Guid userId)
        {
            using (var tx = new TransactionScope())
            {
                MarkExpiredAsCompleted();
                var auction = _auctionRepository.GetById(auctionId);
                if (auction.ExpiresAt <= DateTime.Now)
                {
                    // aukcija se vec zavrsila
                    tx.Complete();
                    return -1;
                }
                if (auction.UserId == userId)
                {
                    // korisnik je vlasnik aukcije
                    tx.Complete();
                    return -2;
                }
                var oldBid = _bidRepository.GetTopBidForAuction(auctionId);
                if (oldBid != null && oldBid.UserId == userId)
                {
                    // korisnik vec vodi na licitaciji
                    tx.Dispose();
                    return -3;
                }
                var user = _userRepository.GetById(userId);
                if ((user.TokenCount - (auction.Price)) < 0)
                {
                    // korisnik nema dovoljno sredstava za licitiranje na datoj aukciji
                    tx.Dispose();
                    return -4;
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
                    Id = Guid.NewGuid(),
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

        public List<DisplayAuctionModel> GetAllWonByUser(Guid userId)
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

        public int GetAuctionPrice(Guid auctionId)
        {
            return _auctionRepository.GetById(auctionId).Price;
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
            var topBid = _bidRepository.GetTopBidForAuction(auction.Id);
            if (topBid != null)
            {
                var highestBidder = _userRepository.GetById(topBid.UserId);
                model.HighestBidder = highestBidder.Username;
            }
            model.H = timeLeft.Hours;
            model.M = timeLeft.Minutes;
            model.S = timeLeft.Seconds;
            model.ExpiresAt = expiresAt;

            return model;
        }

        private void MarkExpiredAsCompleted()
        {
            IList<Auction> auctions = _auctionRepository.GetAllStarted().ToList();
            var completedId = _auctionStatusRepository.GetByType("FINISHED").Id;
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
