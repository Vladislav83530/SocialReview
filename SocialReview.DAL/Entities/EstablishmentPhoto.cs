using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("EstablishmentPhotos")]
    public class EstablishmentPhoto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Photo { get; set; } = string.Empty;

        [Required]
        [ForeignKey("EstablishmentId")]
        public Guid EstablishmentId { get; set; }

        public Establishment Establishment { get; set; }
    }
}
