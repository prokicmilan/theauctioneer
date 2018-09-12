using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using ViewModelLayer.Models.Auction;

namespace BusinessLogicLayer.Repositories
{
    public class AuctionBl
    {

        private readonly AuctionRepository _auctionRepository = new AuctionRepository();

        private readonly AuctionStatusRepository _auctionStatusRepository = new AuctionStatusRepository();
    
        private readonly BidRepository _bidRepository = new BidRepository();

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
            var auctions = _auctionRepository.GetAllStarted().ToList();
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
                ExpiresAt = DateTime.MaxValue,
                Price = model.Price
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
            var auction = _auctionRepository.GetById(model.Id);
            auction.StatusId = _auctionStatusRepository.GetByType("OPENED").Id;
            _auctionRepository.Save(auction);
        }

        public void IncreasePrice(int auctionId, int userId)
        {
            var auction = _auctionRepository.GetById(auctionId);
            var bid = new Bid
            {
                UserId = userId,
                AuctionId = auctionId,
                Timestamp = DateTime.Now,
                BidAmount = auction.Price + 1
            };
            auction.Price++;
            _bidRepository.Save(bid);
            _auctionRepository.Save(auction);
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

            return model;
        }

    }
}
