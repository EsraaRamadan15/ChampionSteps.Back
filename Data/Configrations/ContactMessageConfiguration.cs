using ChampionSteps.Models.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChampionSteps.Data.Configrations
{

    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Type)
                .HasMaxLength(32)
                .IsRequired();

            e.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.Phone)
                .HasMaxLength(40);

            e.Property(x => x.Subject)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.PreferredContact)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.IpAddress)
                .HasMaxLength(64);

            e.Property(x => x.UserAgent)
                .HasMaxLength(512);

            e.HasIndex(x => x.CreatedAtUtc);
        }
    }
}
