using System.ComponentModel.DataAnnotations;

namespace ChampionSteps.Models.Stories
{

    public enum StoryVisibility
    {
        Public = 0,
        PendingReview = 1,
        Rejected = 2
    }

    public enum StorySourceType
    {
        Curated = 0,   // قصصك أنتِ
        User = 1       // قصص المستخدمين
    }

    public class Story
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // مصر / بريطانيا / ...
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        // رياضة / إعلام / تعليم / موضة / ...
        [MaxLength(100)]
        public string Domain { get; set; } = string.Empty;

        // اسم البطل/الشخص
        [MaxLength(200)]
        public string PersonName { get; set; } = string.Empty;

        // القصة/الملخص
        [MaxLength(8000)]
        public string Summary { get; set; } = string.Empty;

        // أهم سطر في القصة
        [MaxLength(500)]
        public string? Highlight { get; set; }

        // كلمات مفتاحية
        [MaxLength(800)]
        public string? TagsCsv { get; set; }

        // ✅ صورة أساسية للقصة (Card / Thumbnail)
        [MaxLength(2000)]
        public string? CoverImageUrl { get; set; }

        public StorySourceType SourceType { get; set; } = StorySourceType.Curated;
        public StoryVisibility Visibility { get; set; } = StoryVisibility.Public;

        // بيانات صاحب الإضافة (اختياري)
        [MaxLength(200)]
        public string? SubmittedByName { get; set; }

        [MaxLength(200)]
        public string? SubmittedByEmail { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public List<StoryMedia> Media { get; set; } = new();
    }

    // ✅ ميديا مرنة: صورة/فيديو/مقال... كلها URLs
    public class StoryMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StoryId { get; set; }
        public Story Story { get; set; } = default!;

        // image | video | article | other
        [MaxLength(20)]
        public string Kind { get; set; } = "other";

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string Url { get; set; } = string.Empty;

        public int Order { get; set; } = 0;
    }

}
