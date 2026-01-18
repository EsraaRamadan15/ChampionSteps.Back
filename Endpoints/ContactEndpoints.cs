using ChampionSteps.Data.Context;
using ChampionSteps.Models.Contact;
using ChampionSteps.Validators.Contact;
using Microsoft.EntityFrameworkCore;

namespace ChampionSteps.Endpoints
{
    public static class ContactEndpoints
    {
        public static IEndpointRouteBuilder MapContactEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/contact").WithTags("Contact");

            // POST /api/contact
            group.MapPost("/", async (ContactRequestDto dto, AppDbContext db, HttpContext http, CancellationToken ct) =>
            {
                var errors = ContactRequestValidator.Validate(dto);
                if (errors.Count > 0)
                    return Results.BadRequest(new { message = "Validation failed", errors });

                ContactRequestValidator.Normalize(dto);

                var msg = new ContactMessage
                {
                    Type = dto.Type,
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Subject = dto.Subject,
                    Message = dto.Message,
                    PreferredContact = dto.PreferredContact ?? "email",
                    Urgent = dto.Urgent,
                    Consent = dto.Consent,
                    CreatedAtUtc = DateTime.UtcNow,
                    IpAddress = http.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = http.Request.Headers.UserAgent.ToString()
                };

                db.ContactMessages.Add(msg);
                await db.SaveChangesAsync(ct);

                return Results.Ok(new { id = msg.Id, message = "تم إرسال رسالتك بنجاح ✅ هنرد عليك قريبًا." });
            });

            // (اختياري) GET /api/contact (للأدمن)
            group.MapGet("/", async (AppDbContext db, int page = 1, int pageSize = 20) =>
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

                var query = db.ContactMessages.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc);

                var total = await query.CountAsync();
                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                return Results.Ok(new { page, pageSize, total, items });
            });

            return app;
        }
    }
}
