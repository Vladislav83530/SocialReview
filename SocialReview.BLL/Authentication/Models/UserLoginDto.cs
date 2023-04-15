using System.ComponentModel.DataAnnotations;


namespace SocialReview.BLL.Authentication.Models
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Wrong email format")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
