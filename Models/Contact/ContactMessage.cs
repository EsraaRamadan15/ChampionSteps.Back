namespace ChampionSteps.Models.Contact
{
    public class ContactMessage
    {
        public long Id { get; set; }

        public string Type { get; set; } = "inquiry"; // inquiry | consultation | feedback
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string PreferredContact { get; set; } = "email"; // email | phone | whatsapp
        public bool Urgent { get; set; }
        public bool Consent { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Metadata مفيد
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

}
