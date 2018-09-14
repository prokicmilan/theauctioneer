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

    }
}
