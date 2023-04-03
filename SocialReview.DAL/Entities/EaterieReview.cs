using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("EaterieReviews")]
    public class EaterieReview
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int FoodQuality { get; set; }

        [Required]
        [Range(1, 5)]
        public int ServiceQuality { get; set; }

        [Required]
        [Range(1, 5)]
        public int PriceQuality { get; set; }

        [Required]
        [Range(1, 5)]
        public int GeneralImpression { get; set; }

        [Required]
        [Range(1, 5)]
        public int Cleanness { get; set; }

        public string? Message { get; set; } = string.Empty;

        [Required]
        public DateTime ReviewDate { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("EstablishmentId")]
        public Guid EstablishmentId { get; set;}

        public Customer User { get; set; }
        public Establishment Establishment { get; set; }
    }
}