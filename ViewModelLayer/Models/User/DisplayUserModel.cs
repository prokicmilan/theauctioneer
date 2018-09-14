using System.ComponentModel;

namespace ViewModelLayer.Models.User
{
    public class DisplayUserModel : BaseUserModel
    {
        
        [DisplayName("Token count")]
        public int TokenCount { get; set; }

    }
}
