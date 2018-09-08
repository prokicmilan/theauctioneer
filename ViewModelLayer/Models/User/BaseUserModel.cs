using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.User
{
    public class BaseUserModel : BaseModel
    {
        [Required(ErrorMessage = "You must enter a name")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [DisplayName("First name")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "You must enter a last name.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [DisplayName("Last name")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "You must choose a gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "You must enter an email address.")]
        [MaxLength(128, ErrorMessage = "Maximum length is 128 characters.")]
        [EmailAddress]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a username.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string Username { get; set; }
    }
}
