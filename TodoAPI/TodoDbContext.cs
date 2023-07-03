using Microsoft.EntityFrameworkCore;

namespace TodoAPI
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; } = null!;

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
    }
}