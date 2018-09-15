using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ViewModelLayer.Models.Auction
{
    public class CreateAuctionModel : BaseAuctionModel
    {

        [Required(ErrorMessage = "You must upload a picture.")]
        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [DisplayName("Duration in seconds")]
        [Range(1, 604800, ErrorMessage = "Value must be a positive integer, auctions cannot last more than 7 days.")]
        public long Duration { get; set; }

    }
}
