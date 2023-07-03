using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TodoAPI;

public class Program
{
   public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("TodoDb"));
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.MapPost("/todo", (Todo todo, TodoDbContext db) =>
        {
            // Save todo to the database
            db.Todos.Add(todo);
            db.SaveChanges();
            return Results.Created($"/todo/{todo.Id}", todo);
        });

        app.MapGet("/todo", async (TodoDbContext db) =>
            await db.Todos.ToListAsync()
        );

        app.MapGet("/todo/{id}", async (int id, TodoDbContext db) =>
            await db.Todos.FindAsync(id)
            is Todo todo ?
                Results.Ok(todo) :
                Results.NotFound()
        );


        app.Run();
    }
}