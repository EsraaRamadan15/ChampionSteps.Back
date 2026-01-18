namespace ChampionSteps.Endpoints
{
    using ChampionSteps.Data.Context;
    using ChampionSteps.Models;
    using Microsoft.EntityFrameworkCore;

    public static class TodoEndpoints
    {
        public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/todos").WithTags("Todos");

            // GET /api/todos
            group.MapGet("/", async (AppDbContext db) =>
                await db.TodoItems.AsNoTracking().ToListAsync());

            // GET /api/todos/{id}
            group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
                await db.TodoItems.FindAsync(id) is { } todo ? Results.Ok(todo) : Results.NotFound());

            // POST /api/todos
            group.MapPost("/", async (TodoItem input, AppDbContext db) =>
            {
                db.TodoItems.Add(input);
                await db.SaveChangesAsync();
                return Results.Created($"/api/todos/{input.Id}", input);
            });

            // PUT /api/todos/{id}
            group.MapPut("/{id:int}", async (int id, TodoItem input, AppDbContext db) =>
            {
                var todo = await db.TodoItems.FindAsync(id);
                if (todo is null) return Results.NotFound();

                todo.Title = input.Title;
                todo.IsDone = input.IsDone;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE /api/todos/{id}
            group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
            {
                var todo = await db.TodoItems.FindAsync(id);
                if (todo is null) return Results.NotFound();

                db.TodoItems.Remove(todo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }

}
