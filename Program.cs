using ChampionSteps.Data.Context;
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

// CRUD
app.MapGet("/api/todos", async (AppDbContext db) =>
    await db.TodoItems.AsNoTracking().ToListAsync());

app.MapGet("/api/todos/{id:int}", async (int id, AppDbContext db) =>
    await db.TodoItems.FindAsync(id) is { } todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/api/todos", async (TodoItem input, AppDbContext db) =>
{
    db.TodoItems.Add(input);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{input.Id}", input);
});

app.MapPut("/api/todos/{id:int}", async (int id, TodoItem input, AppDbContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    todo.Title = input.Title;
    todo.IsDone = input.IsDone;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/todos/{id:int}", async (int id, AppDbContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.TodoItems.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
