using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("EaterieReviews")]
    public class EaterieReview : Review
    {
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
    }
}