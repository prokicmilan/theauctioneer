using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Classes
{
    [Table("Bid")]
    public partial class Bid
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int AuctionId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int BidAmount { get; set; }

    }
}
