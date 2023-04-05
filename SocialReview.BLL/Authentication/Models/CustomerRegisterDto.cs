using System.ComponentModel.DataAnnotations;

namespace SocialReview.BLL.Authentication.Models
{
    public class CustomerRegisterDto : UserCredentialsDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Wrong format of phone number. Must be '+380xxxxxxxxx'")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
