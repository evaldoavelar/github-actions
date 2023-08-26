
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TodoAPI;
using System.Net.Http.Json;
using System.Net;

namespace TodoTests.IntegratedTests
{
    public class TodoEndpointsV1 : IClassFixture<WebApplicationFactory<TodoAPI.Program>>
    {
        private readonly WebApplicationFactory<TodoAPI.Program> _factory;
        private readonly HttpClient _httpClient = null!;

        public TodoEndpointsV1(WebApplicationFactory<TodoAPI.Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        public static IEnumerable<Object[]> GetValidTodo => new List<Object[]>(){
           new object[]{ new Todo() { Title = "Teste1", Description = "Teste Description 1" }}
        };

        public static IEnumerable<Object[]> GetValidTodos => new List<Object[]>(){
           new object[]{ new Todo() { Title = "Teste1", Description = "Teste Description 1" }},
           new object[]{ new Todo() { Title = "Teste2", Description = "Teste Description 2" }},
           new object[]{ new Todo() { Title = "Teste3", Description = "Teste Description 3" }}
        };

        public static IEnumerable<Object[]> GetInvalidsTodos => new List<Object[]>(){
            new object[]{ new Todo() { Title = "Valid Todo", Description = "Teste Valid 1" }},
            new object[]{ new Todo(){Description = "No name"}}
        };

        private void SaveTodo(Todo todo)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            db.Add(todo);
            db.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(GetValidTodo))]
        public async Task PostTodoWithValidParametersAsync(Todo todo)
        {
            var result = await _httpClient.PostAsJsonAsync("/todo", todo);
            Assert.True(result.IsSuccessStatusCode);

            var todoJson = await result.Content.ReadFromJsonAsync<Todo>();
            Assert.NotNull(todoJson);
            Assert.NotEqual(0, todoJson.Id);
            Assert.Equal(todo.Title, todoJson.Title);
            Assert.Equal(todo.Description, todoJson.Description);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            var todoBD = db.Todos.Find(todoJson.Id);

            Assert.NotNull(todoBD);
            Assert.Equal(todo.Title, todoBD.Title);
            Assert.Equal(todo.Description, todoBD.Description);

        }

        //post invalid todo
        [Fact]
        public async Task PostTodoWithInvalidParametersAsync()
        {
            Todo todo = null!;
            var result = await _httpClient.PostAsJsonAsync("/todo", todo);
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Theory]
        [MemberData(nameof(GetValidTodo))]
        public async Task GetByIdTodoWithValidParametersAsync(Todo todo)
        {
            SaveTodo(todo);

            var result = await _httpClient.GetAsync($"/todo/{todo.Id}");

            Assert.True(result.IsSuccessStatusCode);

            var todoJson = await result.Content.ReadFromJsonAsync<Todo>();
            Assert.NotNull(todoJson);
            Assert.Equal(todo.Id, todoJson.Id);
            Assert.Equal(todo.Title, todoJson.Title);
            Assert.Equal(todo.Description, todoJson.Description);
        }



        [Fact]
        public async Task GetByIdTodoWithInvalidParametersAsync()
        {
            // act
            var result = await _httpClient.GetAsync($"/todo/0");
            // assert
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Theory]
        [MemberData(nameof(GetValidTodos))]
        public async Task GetAllTodoAsync(Todo todo)
        {
            // arrange
            SaveTodo(todo);

            // act
            var result = await _httpClient.GetAsync($"/todo");

            // assert
            Assert.True(result.IsSuccessStatusCode);

            var todoJson = await result.Content.ReadFromJsonAsync<List<Todo>>();

            Assert.NotNull(todoJson);
            Assert.NotEmpty(todoJson);

            Assert.All(todoJson, (todo) =>
            {
                Assert.NotNull(todo);
                Assert.NotEqual(0, todo.Id);
                Assert.NotEmpty(todo.Title);
                Assert.NotEmpty(todo.Description);
            });

        }

        [Theory]
        [MemberData(nameof(GetValidTodo))]
        public async void UpdateTodo(Todo todo)
        {
            // Given
            SaveTodo(todo);
            // When
            todo.Title = "Updated Title";
            var result = await _httpClient.PutAsJsonAsync($"/todo", todo);
            // Then
            var todoAPI = await result.Content.ReadFromJsonAsync<Todo>();
            Assert.True(result.IsSuccessStatusCode);
            Assert.NotNull(todoAPI);
            Assert.Equal(todo.Title, todoAPI?.Title);
            
        }

        [Theory]
        [MemberData(nameof(GetValidTodo))]
        public async void TryUpdateNonExistentTodo(Todo todo)
        {         
            // When
            todo.Title = "Updated Title";
            var result = await _httpClient.PutAsJsonAsync($"/todo", todo);
            
            // Then
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            
        }

    }
}