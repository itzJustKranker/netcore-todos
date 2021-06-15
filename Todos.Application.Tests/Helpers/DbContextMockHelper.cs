using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Todos.Application.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DbContextMockHelper 
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T: class
        {
            var mockDbSet = sourceList.AsQueryable().BuildMockDbSet();
            return mockDbSet;
        }
    }
}