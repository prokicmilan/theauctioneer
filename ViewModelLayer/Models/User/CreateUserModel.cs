using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModelLayer.Models.User
{
    public class CreateUserModel : BaseUserModel
    {

        [Required(ErrorMessage = "You must enter a password")]
        [MinLength(8, ErrorMessage = "Minimum length is 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must confirm the password.")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

    }
}
