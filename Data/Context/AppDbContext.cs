using ChampionSteps.Models;
using Microsoft.EntityFrameworkCore;

namespace ChampionSteps.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
