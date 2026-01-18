using ChampionSteps.Models.Contact;
using System.Net.Mail;

namespace ChampionSteps.Validators.Contact
{

    public static class ContactRequestValidator
    {
        private static readonly HashSet<string> AllowedTypes =
            new(StringComparer.OrdinalIgnoreCase) { "inquiry", "consultation", "feedback" };

        private static readonly HashSet<string> AllowedPreferred =
            new(StringComparer.OrdinalIgnoreCase) { "email", "phone", "whatsapp" };

        public static List<string> Validate(ContactRequestDto dto)
        {
            var errors = new List<string>();

            if (dto is null) { errors.Add("Body is required."); return errors; }

            if (string.IsNullOrWhiteSpace(dto.Type) || !AllowedTypes.Contains(dto.Type))
                errors.Add("Type must be one of: inquiry, consultation, feedback.");

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Trim().Length < 2)
                errors.Add("Name is required (min 2 chars).");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email is required.");
            else
            {
                try { _ = new MailAddress(dto.Email.Trim()); }
                catch { errors.Add("Email is invalid."); }
            }

            if (string.IsNullOrWhiteSpace(dto.Subject) || dto.Subject.Trim().Length < 3)
                errors.Add("Subject is required (min 3 chars).");

            if (string.IsNullOrWhiteSpace(dto.Message) || dto.Message.Trim().Length < 10)
                errors.Add("Message is required (min 10 chars).");

            var preferred = dto.PreferredContact ?? "email";
            if (!AllowedPreferred.Contains(preferred))
                errors.Add("PreferredContact must be one of: email, phone, whatsapp.");

            if (dto.Consent != true)
                errors.Add("Consent must be true.");

            return errors;
        }

        public static void Normalize(ContactRequestDto dto)
        {
            dto.Type = (dto.Type ?? "inquiry").Trim().ToLowerInvariant();
            dto.Name = (dto.Name ?? "").Trim();
            dto.Email = (dto.Email ?? "").Trim();
            dto.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();
            dto.Subject = (dto.Subject ?? "").Trim();
            dto.Message = (dto.Message ?? "").Trim();
            dto.PreferredContact = (dto.PreferredContact ?? "email").Trim().ToLowerInvariant();
        }
    }

}
