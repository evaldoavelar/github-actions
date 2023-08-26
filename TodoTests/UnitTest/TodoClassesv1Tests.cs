using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoAPI;
using Xunit;

namespace TodoTests.UnitTest
{
    public class TodoClassesv1Tests : Program
    {
        private readonly Todo todoAPI;

        public TodoClassesv1Tests() => todoAPI = new Todo() { Title = "Teste1", Description = "Teste Description 1" };

        

        [Theory]
        [InlineData("Teste1")]
        public void Title_Equal_Teste1(string value) =>
           Assert.True(todoAPI.Title.Equals(value), $"{value} should be prime");
    }
}