namespace DataAccessLayer.Classes
{
    using System.Data.Entity;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("DataModel")
        {
        }

        public virtual DbSet<Auction> Auctions { get; set; }

        public virtual DbSet<AuctionStatus> AuctionStatuses { get; set; }

        public virtual DbSet<Bid> Bids { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<TokenOrder> TokenOrders { get; set; }

        public virtual DbSet<TokenOrderStatus> TokenOrderStatuses { get; set; }

        public virtual DbSet<User> Users { get; set; }

    }
}
