using ChampionSteps.Data.Context;
using ChampionSteps.Models.Stories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChampionSteps.Endpoints
{
    public static class StoriesEndpoints
    {

        public static IEndpointRouteBuilder MapToStoriespoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/stories", async (
    AppDbContext db,
    string? q,
    string? country,
    string? domain,
    StorySourceType? sourceType,
    StoryVisibility? visibility,
    int page = 1,
    int pageSize = 20
) =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var query = db.Stories.AsNoTracking().Include(s => s.Media).AsQueryable();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim();
                    query = query.Where(s =>
                        s.Title.Contains(term) ||
                        s.PersonName.Contains(term) ||
                        s.Summary.Contains(term) ||
                        (s.TagsCsv != null && s.TagsCsv.Contains(term))
                    );
                }

                if (!string.IsNullOrWhiteSpace(country)) query = query.Where(s => s.Country == country.Trim());
                if (!string.IsNullOrWhiteSpace(domain)) query = query.Where(s => s.Domain == domain.Trim());
                if (sourceType is not null) query = query.Where(s => s.SourceType == sourceType);
                if (visibility is not null) query = query.Where(s => s.Visibility == visibility);

                var total = await query.CountAsync();

                var items = await query
                    .OrderByDescending(s => s.CreatedAtUtc)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new StoryDto(
                        s.Id.ToString(),
                        s.Title,
                        s.Domain,
                        s.PersonName,
                        s.Highlight,
                        s.CoverImageUrl
                    ))
                    .ToListAsync();

                return Results.Ok(new { total, page, pageSize, items });
            });

            // 2) Get single story
            app.MapGet("/api/stories/{id:guid}", async (AppDbContext db, Guid id) =>
            {
                var story = await db.Stories.AsNoTracking()
                    .Include(s => s.Media)
                    .FirstOrDefaultAsync(s => s.Id == id);

                return story is null ? Results.NotFound() : Results.Ok(new
                {
                    story.Id,
                    story.Title,
                    story.Country,
                    story.Domain,
                    story.PersonName,
                    story.Summary,
                    story.Highlight,
                    story.TagsCsv,
                    story.CoverImageUrl, // ✅
                    story.SourceType,
                    story.Visibility,
                    story.SubmittedByName,
                    story.SubmittedByEmail,
                    story.CreatedAtUtc,
                    story.UpdatedAtUtc,
                    Media = story.Media.OrderBy(m => m.Order).Select(m => new { m.Kind, m.Title, m.Url, m.Order })
                });
            });

            // 3) Create user story (PendingReview)
            app.MapPost("/api/stories", async (AppDbContext db, StoryCreateDto dto) =>
            {
                var now = DateTime.UtcNow;

                var story = new Story
                {
                    Title = dto.Title.Trim(),
                    Country = dto.Country.Trim(),
                    Domain = dto.Domain.Trim(),
                    PersonName = dto.PersonName.Trim(),
                    Summary = dto.Summary.Trim(),
                    Highlight = dto.Highlight?.Trim(),
                    TagsCsv = dto.TagsCsv?.Trim(),
                    CoverImageUrl = dto.CoverImageUrl?.Trim(), // ✅
                    SourceType = StorySourceType.User,
                    Visibility = StoryVisibility.PendingReview,
                    SubmittedByName = dto.SubmittedByName?.Trim(),
                    SubmittedByEmail = dto.SubmittedByEmail?.Trim(),
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                };

                if (dto.Media?.Any() == true)
                {
                    story.Media = dto.Media.Select(m => new StoryMedia
                    {
                        Kind = NormalizeKind(m.Kind),
                        Title = m.Title?.Trim(),
                        Url = m.Url.Trim(),
                        Order = m.Order
                    }).ToList();
                }

                db.Stories.Add(story);
                await db.SaveChangesAsync();

                return Results.Created($"/api/stories/{story.Id}", new { story.Id });
            });

            // 4) Update story (admin in demo)
            app.MapPut("/api/stories/{id:guid}", async (AppDbContext db, Guid id, StoryUpdateDto dto) =>
            {
                var story = await db.Stories.Include(s => s.Media).FirstOrDefaultAsync(s => s.Id == id);
                if (story is null) return Results.NotFound();

                story.Title = dto.Title.Trim();
                story.Country = dto.Country.Trim();
                story.Domain = dto.Domain.Trim();
                story.PersonName = dto.PersonName.Trim();
                story.Summary = dto.Summary.Trim();
                story.Highlight = dto.Highlight?.Trim();
                story.TagsCsv = dto.TagsCsv?.Trim();
                story.CoverImageUrl = dto.CoverImageUrl?.Trim(); // ✅
                story.Visibility = dto.Visibility;
                story.UpdatedAtUtc = DateTime.UtcNow;

                // replace media
                db.StoryMedia.RemoveRange(story.Media);
                story.Media.Clear();

                if (dto.Media?.Any() == true)
                {
                    story.Media.AddRange(dto.Media.Select(m => new StoryMedia
                    {
                        StoryId = story.Id,
                        Kind = NormalizeKind(m.Kind),
                        Title = m.Title?.Trim(),
                        Url = m.Url.Trim(),
                        Order = m.Order
                    }));
                }

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // 5) Delete story
            app.MapDelete("/api/stories/{id:guid}", async (AppDbContext db, Guid id) =>
            {
                var story = await db.Stories.Include(s => s.Media).FirstOrDefaultAsync(s => s.Id == id);
                if (story is null) return Results.NotFound();

                db.Stories.Remove(story);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });



            // ... داخل Program.cs بعد باقي الـ endpoints أو قبل app.Run()

            app.MapPost("/api/stories/bulk", async (AppDbContext db, List<StoryCreateDto> items) =>
            {
                if (items is null || items.Count == 0)
                    return Results.BadRequest(new { message = "البيانات المرسلة فارغة" });

                var now = DateTime.UtcNow;

                static string NormalizeKind(string? kind)
                {
                    var k = (kind ?? "other").Trim().ToLowerInvariant();
                    return k switch
                    {
                        "image" => "image",
                        "video" => "video",
                        "article" => "article",
                        _ => "other"
                    };
                }

                var stories = items.Select(dto => new Story
                {
                    Title = dto.Title.Trim(),
                    Country = dto.Country.Trim(),
                    Domain = dto.Domain.Trim(),
                    PersonName = dto.PersonName.Trim(),
                    Summary = dto.Summary.Trim(),
                    Highlight = dto.Highlight?.Trim(),
                    TagsCsv = dto.TagsCsv?.Trim(),
                    CoverImageUrl = dto.CoverImageUrl?.Trim(),

                    SourceType = StorySourceType.Curated,
                    Visibility = StoryVisibility.Public,

                    SubmittedByName = dto.SubmittedByName?.Trim(),
                    SubmittedByEmail = dto.SubmittedByEmail?.Trim(),

                    CreatedAtUtc = now,
                    UpdatedAtUtc = now,

                    Media = dto.Media?.Select(m => new StoryMedia
                    {
                        Kind = NormalizeKind(m.Kind),
                        Title = m.Title?.Trim(),
                        Url = m.Url.Trim(),
                        Order = m.Order
                    }).ToList() ?? new List<StoryMedia>()
                }).ToList();

                db.Stories.AddRange(stories);
                await db.SaveChangesAsync();

                return Results.Ok(new
                {
                    inserted = stories.Count,
                    ids = stories.Select(s => s.Id).ToList()
                });
            });




            return app;
        }
        static string NormalizeKind(string? kind)
        {
            var k = (kind ?? "other").Trim().ToLowerInvariant();
            return k switch
            {
                "image" => "image",
                "video" => "video",
                "article" => "article",
                _ => "other"
            };
        }

    }
}
