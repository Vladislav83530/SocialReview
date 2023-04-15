using System.ComponentModel.DataAnnotations;

namespace SocialReview.BLL.Authentication.Models
{
    public class CustomerRegisterDto : UserCredentialsDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
