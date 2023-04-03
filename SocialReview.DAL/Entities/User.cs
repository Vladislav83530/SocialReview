using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialReview.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public Role Role { get; set; }

        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid? EstablishmentId { get; set; }
        public Establishment? Establishment { get; set; }
    }

    public enum Role
    {
        Customer,
        Establishment
    }
}
