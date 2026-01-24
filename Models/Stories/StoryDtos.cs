using System.ComponentModel.DataAnnotations;

namespace ChampionSteps.Models.Stories
{

    public record StoryMediaDto(
        string Kind,      // image | video | article | other
        string? Title,
        string Url,
        int Order = 0
    );



    public record StoryDto
(
        string Id,
   string Title,

     string Domain,
 string PersonName,
    string? Highlight,
    string? CoverImageUrl         // ✅ صورة أساسية
);
    public record StoryCreateDto
    (
        [Required, MaxLength(200)] string Title,
        [Required, MaxLength(100)] string Country,
        [Required, MaxLength(100)] string Domain,
        [Required, MaxLength(200)] string PersonName,
        [Required, MaxLength(8000)] string Summary,
        [MaxLength(500)] string? Highlight,
        [MaxLength(800)] string? TagsCsv,
        [MaxLength(2000)] string? CoverImageUrl,          // ✅ صورة أساسية
        List<StoryMediaDto>? Media,                       // ✅ صور/فيديو/مقالات
        string? SubmittedByName,
        string? SubmittedByEmail
    );

    public record StoryUpdateDto
    (
        [Required, MaxLength(200)] string Title,
        [Required, MaxLength(100)] string Country,
        [Required, MaxLength(100)] string Domain,
        [Required, MaxLength(200)] string PersonName,
        [Required, MaxLength(8000)] string Summary,
        [MaxLength(500)] string? Highlight,
        [MaxLength(800)] string? TagsCsv,
        [MaxLength(2000)] string? CoverImageUrl,          // ✅
        List<StoryMediaDto>? Media,                       // ✅
        StoryVisibility Visibility
    );

}
