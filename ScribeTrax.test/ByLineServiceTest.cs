using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using ScribeTrax.ViewModels;
using System.Collections.Generic;
using System.Linq;

public class BylineServiceTests
{
    private List<Byline> GetFakeBylines() => new()
    {
        new Byline { BylineId = 1, Name = "Tommy Bolin", Type = "Solo", Inactive = false },
        new Byline { BylineId = 2, Name = "Energy", Type = "Band", Inactive = true },
        new Byline { BylineId = 3, Name = "Billy Cobham", Type = "Collab", Inactive = null }
    };

    private Mock<DbSet<Byline>> GetMockSet(List<Byline> data)
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<Byline>>();
        mockSet.As<IQueryable<Byline>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Byline>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Byline>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Byline>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockSet;
    }

    [Fact]
    public void GetAllBylines_ExcludesInactive_WhenFlagFalse()
    {
        var data = GetFakeBylines();
        var mockSet = GetMockSet(data);
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines).Returns(mockSet.Object);

        var service = new BylineService(mockContext.Object);
        var result = service.GetAllBylines(includeInactive: false);

        result.Should().HaveCount(2);
        result.Should().NotContain(b => b.Name == "Energy");
    }

    [Fact]
    public void GetAllBylines_IncludesInactive_WhenFlagTrue()
    {
        var data = GetFakeBylines();
        var mockSet = GetMockSet(data);
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines).Returns(mockSet.Object);

        var service = new BylineService(mockContext.Object);
        var result = service.GetAllBylines(includeInactive: true);

        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetBylineById_ReturnsCorrectByline()
    {
        var data = GetFakeBylines();
        var mockSet = GetMockSet(data);
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines.Find(1)).Returns(data.First());

        var service = new BylineService(mockContext.Object);
        var result = service.GetBylineById(1);

        result.Should().NotBeNull();
        result.Name.Should().Be("Tommy Bolin");
    }

    [Fact]
    public void CreateByline_AddsEntityAndSaves()
    {
        var mockSet = new Mock<DbSet<Byline>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines).Returns(mockSet.Object);

        var service = new BylineService(mockContext.Object);
        var model = new BylineCreateModel { Name = "Alphonse Mouzon", Type = "Fusion", IsInactive = false };

        service.CreateByline(model);

        mockSet.Verify(m => m.Add(It.IsAny<Byline>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateByline_UpdatesFieldsAndSaves()
    {
        var entity = new Byline { BylineId = 1, Name = "Old Name", Type = "Old Type", Inactive = false };
        var mockSet = new Mock<DbSet<Byline>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines.Find(1)).Returns(entity);

        var service = new BylineService(mockContext.Object);
        var model = new BylineUpdateModel { Name = "New Name", Type = "New Type", IsInactive = true };

        service.UpdateByline(1, model);

        entity.Name.Should().Be("New Name");
        entity.Type.Should().Be("New Type");
        entity.Inactive.Should().BeTrue();
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeactivateByline_SetsInactiveTrueAndSaves()
    {
        var entity = new Byline { BylineId = 1, Name = "Active Guy", Inactive = false };
        var mockSet = new Mock<DbSet<Byline>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Bylines.Find(1)).Returns(entity);

        var service = new BylineService(mockContext.Object);
        service.DeactivateByline(1);

        entity.Inactive.Should().BeTrue();
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}