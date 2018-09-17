using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Classes
{
    [Table("Bid")]
    public partial class Bid
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AuctionId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int BidAmount { get; set; }

    }
}
