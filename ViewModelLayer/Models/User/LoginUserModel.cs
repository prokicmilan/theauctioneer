using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModelLayer.Models.User
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "You have to enter a username.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "You have to enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}
