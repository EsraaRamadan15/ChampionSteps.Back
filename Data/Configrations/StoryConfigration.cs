using ChampionSteps.Models.Contact;
using ChampionSteps.Models.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChampionSteps.Data.Configrations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {


        public void Configure(EntityTypeBuilder<Story> e)
        {


            e.HasIndex(x => x.Country);
            e.HasIndex(x => x.Domain);
            e.HasIndex(x => x.PersonName);
            e.HasIndex(x => x.SourceType);
            e.HasIndex(x => x.Visibility);

            e.Property(x => x.CreatedAtUtc)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            e.Property(x => x.UpdatedAtUtc)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }

        public class StoryMediaConfiguration : IEntityTypeConfiguration<StoryMedia>
        {

            public void Configure(EntityTypeBuilder<StoryMedia> e)
            {
                e.HasIndex(x => x.StoryId);
                e.HasIndex(x => x.Url);
                e.Property(x => x.Url).IsRequired();

                // علاقة 1..N
                e.HasOne(m => m.Story)
                 .WithMany(s => s.Media)
                 .HasForeignKey(m => m.StoryId)
                 .OnDelete(DeleteBehavior.Cascade);
            }

        }
    }
}

