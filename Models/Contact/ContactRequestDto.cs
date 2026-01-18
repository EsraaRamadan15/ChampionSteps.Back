using System.ComponentModel.DataAnnotations;

namespace ChampionSteps.Models.Contact
{
    public class ContactRequestDto
    {
        [Required]
        [RegularExpression("inquiry|consultation|feedback")]
        public string Type { get; set; } = "inquiry";

        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required, MinLength(3)]
        public string Subject { get; set; } = string.Empty;

        [Required, MinLength(10)]
        public string Message { get; set; } = string.Empty;

        [RegularExpression("email|phone|whatsapp")]
        public string? PreferredContact { get; set; } = "email";

        public bool Urgent { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Consent must be true")]
        public bool Consent { get; set; }
    }

}
