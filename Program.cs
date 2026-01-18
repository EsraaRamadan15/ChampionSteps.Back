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

var app = builder.Build();

// Auto-migrate on startup (good for demo)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapTodoEndpoints();
app.MapContactEndpoints();

app.Run();
