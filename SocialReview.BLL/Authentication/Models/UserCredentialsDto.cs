﻿using System.ComponentModel.DataAnnotations;

namespace SocialReview.BLL.Authentication.Models
{
    public class UserCredentialsDto
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Wrong email format")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Wrong format of phone number. Must be '+380xxxxxxxxx'")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
