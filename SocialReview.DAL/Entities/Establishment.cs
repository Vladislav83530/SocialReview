using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("Establishments")]
    public class Establishment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? MainPhoto { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public string? PhoneNumber { get; set;} = string.Empty;
    }
}
