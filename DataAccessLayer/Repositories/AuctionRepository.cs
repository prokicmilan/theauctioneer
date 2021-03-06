﻿using System;
using System.Linq;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class AuctionRepository : EditableRepositoryBase<Auction>
    { 

        public IQueryable<Auction> GetAllReady()
        {
            /*
             * select
             *      Auction.*
             * from
             *      Auction
             * join
             *      AuctionStatus
             * on
             *      AuctionStatus.Id = Auction.StatusId
             * where
             *      AuctionStatus.[Type] = 'READY'
             */
            return context.Auctions.Join(context.AuctionStatuses,                                                     // tabela za spajanje
                                                auction => auction.StatusId,                                          // strani kljuc
                                                status => status.Id,                                                  // primarni kljuc
                                                (auction, status) => new {Auction = auction, AuctionStatus = status}) // agregacija
                                   .Where(status => status.AuctionStatus.Type.Equals("READY"))                        // where
                                   .Select(auction => auction.Auction);                                               // select
        }

        public IQueryable<Auction> GetAllStarted()
        {
            /*
             * select
             *      Auction.*
             * from
             *      Auction
             * join
             *      AuctionStatus
             * on
             *      AuctionStatus.Id = Auction.StatusId
             * where
             *      AuctionStatus.[Type] = 'OPENED'
             */
            return context.Auctions.Join(context.AuctionStatuses,
                                         auction => auction.StatusId,
                                         status => status.Id,
                                         (auction, status) => new { Auction = auction, AuctionStatus = status })
                                   .Where(status => status.AuctionStatus.Type.Equals("OPENED"))
                                   .Select(auction => auction.Auction);
        }

        public IQueryable<Auction> FilterStarted(int? priceLow, int? priceHigh, string searchString)
        {
            // dohvatamo sve pokrenute aukcije
            var auctions = GetAllStarted();
            if (priceLow != null)
            {
                // izdvajamo aukcije sa cenom vecom i jednakom minimumu
                auctions = auctions.Where(auction => auction.Price >= priceLow);
            }
            if (priceHigh != null)
            {
                // izdvajamo aukcije sa cenom manjom i jednakom maksimumu
                auctions = auctions.Where(auction => auction.Price <= priceHigh);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                // izdvajamo aukcije sa imenom slicnim prosledjenom parametru za pretragu
                auctions = auctions.Where(auction => auction.Name.Contains(searchString));
            }
            return auctions;
        }

        public IQueryable<Auction> GetAllWonByUser(Guid userId)
        {
            /*
             * select
             *      [a].*
             *  from
             *      Auction [a]
             *  join
             *      AuctionStatus [as]
             *  on
             *      [as].Id = [a].StatusId
             *  and
             *      [as].Type = 'COMPLETED'
             *  join
             *      Bid [b]
             *  on
             *      [b].AuctionId = [a].Id
             *  and
             *      [b].UserId = @userId
             *  where
             *      [b].BidAmount = (select
             *			                max([b2].BidAmount)
             * 		                 from
             *      		            Bid [b2]
             *  		             where
             *  			            [b2].AuctionId = [b].AuctionId)
             */
            var query = from auction in context.Auctions
                        join status in context.AuctionStatuses 
                        on new { auction.StatusId, Type = "COMPLETED" } equals new { StatusId = status.Id, status.Type } 
                        join bid in context.Bids
                        on new { AuctionId = auction.Id, UserId = userId } equals new { bid.AuctionId, bid.UserId }
                        where bid.BidAmount == (from bidAggr in context.Bids
                                                where bidAggr.AuctionId == bid.AuctionId
                                                select bidAggr.BidAmount).Max()
                        select auction;

            return query;
        }

        public IQueryable<Auction> GetAllCreatedByUser(Guid userId)
        {
            return context.Auctions.Where(auction => auction.UserId == userId);
        }
    }
}
