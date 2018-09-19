using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Classes
{
    [Table("TokenOrder")]
    public partial class TokenOrder
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        [Required]
        public DateTime TimestampCreated { get; set; }

        [Required]
        public DateTime TimestampChanged { get; set; }

    }
}
