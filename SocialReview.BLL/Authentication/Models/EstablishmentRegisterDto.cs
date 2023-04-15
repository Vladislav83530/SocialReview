using System.ComponentModel.DataAnnotations;

namespace SocialReview.BLL.Authentication.Models
{
    public class EstablishmentRegisterDto : UserCredentialsDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
