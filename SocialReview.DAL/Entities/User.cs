using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public bool IsVerified { get; set; }

        [Required]
        public string VerificationCode { get; set; } = string.Empty;

        [ForeignKey("CustomerId")]
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey("EstablishmentId")]
        public Guid? EstablishmentId { get; set; }
        public Establishment? Establishment { get; set; }
    }

    public enum Role
    {
        Customer,
        Establishment
    }
}
