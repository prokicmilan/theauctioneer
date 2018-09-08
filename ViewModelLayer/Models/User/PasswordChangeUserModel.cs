using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.User
{
    public class PasswordChangeUserModel : BaseModel
    {
        
        [Required(ErrorMessage = "You must enter your old password")]
        [DisplayName("Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "You must enter a new password")]
        [MinLength(8, ErrorMessage = "Minimum length is 8 characters.")]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "You must confirm the password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords must match.")]
        [DisplayName("Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
