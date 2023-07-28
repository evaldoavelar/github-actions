using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoAPI;
using Xunit;

namespace TodoTests.UnitTest
{
    public class TodoClassesv1Tests
    {
        [Fact]
        public void CouldSaveTodo()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("TodoDb")
                .Options;
            var todo = new Todo() { Title = "Teste1", Description = "Teste Description 1" };
            var db = new TodoDbContext(options);
            db.Todos.Add(todo);
            db.SaveChanges();
            Assert.NotEqual(0, todo.Id);
        }
    }
}