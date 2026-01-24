using ChampionSteps.Data.Context;
using ChampionSteps.Endpoints;
using ChampionSteps.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite (local file)
var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(cs));
// ✅ CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy.WithOrigins(
                "http://localhost:4200",
                "https://khatwat-batl.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});


var app = builder.Build();

// Auto-migrate on startup (good for demo)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
// ✅ must be before MapGet/MapPost...
app.UseCors("frontend");
//app.MapTodoEndpoints();
app.MapContactEndpoints();
app.MapToStoriespoints();

app.Run();
