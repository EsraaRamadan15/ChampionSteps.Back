using ChampionSteps.Models;
using ChampionSteps.Models.Contact;
using ChampionSteps.Models.Stories;
using Microsoft.EntityFrameworkCore;

namespace ChampionSteps.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
        public DbSet<Story> Stories => Set<Story>();
        public DbSet<StoryMedia> StoryMedia => Set<StoryMedia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // يلم كل Configurations الموجودة في نفس الـAssembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
