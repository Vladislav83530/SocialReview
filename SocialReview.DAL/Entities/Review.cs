using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        public string? Message { get; set; } = string.Empty;

        [Required]
        public DateTime ReviewDate { get; set; }

        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }

        [ForeignKey("EstablishmentId")]
        public Guid EstablishmentId { get; set; } 

        public Customer User { get; set; }
        public Establishment Establishment { get; set; }
    }
}
