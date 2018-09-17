using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Classes
{
    [Table("Auction")]
    public partial class Auction
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public long Duration { get; set; }

        public DateTime ExpiresAt { get; set; }

        public Guid StatusId { get; set; }

    }
}
