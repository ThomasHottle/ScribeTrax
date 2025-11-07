using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using System.Collections.Generic;
using System.Linq;

public class MarketServiceTests
{
    private List<Market> GetFakeMarkets() => new()
    {
        new Market { MarketId = 1, Name = "SciFi Weekly", Editor = "Jane", Type = "Magazine", Email = "jane@scifi.com", Url = "http://scifi.com", Postal = "12345" },
        new Market { MarketId = 2, Name = "Fantasy Digest", Editor = "Bob", Type = "Web", Email = "bob@fantasy.com", Url = "http://fantasy.com", Postal = "67890" }
    };

    private Mock<DbSet<Market>> GetMockSet(List<Market> data)
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<Market>>();
        mockSet.As<IQueryable<Market>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Market>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Market>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Market>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockSet;
    }

    [Fact]
    public void GetAllMarkets_ReturnsAllMarkets()
    {
        var data = GetFakeMarkets();
        var mockSet = GetMockSet(data);
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);

        var service = new MarketService(mockContext.Object);
        var result = service.GetAllMarkets();

        result.Should().HaveCount(2);
        result.Should().Contain(m => m.Name == "SciFi Weekly");
    }

    [Fact]
    public void GetMarketById_ReturnsCorrectMarket()
    {
        var data = GetFakeMarkets();
        var mockSet = GetMockSet(data);
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);
        mockContext.Setup(c => c.Markets.FirstOrDefault(It.IsAny<System.Func<Market, bool>>()))
                   .Returns((System.Func<Market, bool> predicate) => data.FirstOrDefault(predicate));

        var service = new MarketService(mockContext.Object);
        var result = service.GetMarketById(1);

        result.Should().NotBeNull();
        result.Name.Should().Be("SciFi Weekly");
    }

    [Fact]
    public void UpdateMarket_UpdatesFieldsAndSaves()
    {
        var entity = new Market { MarketId = 1, Name = "Old Name", Editor = "Old Editor", Type = "Old Type", Email = "old@email.com", Url = "http://old.com", Postal = "00000" };
        var mockSet = new Mock<DbSet<Market>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);
        mockContext.Setup(c => c.Markets.FirstOrDefault(m => m.MarketId == 1)).Returns(entity);

        var service = new MarketService(mockContext.Object);
        var model = new MarketViewModel
        {
            MarketId = 1,
            Name = "New Name",
            Editor = "New Editor",
            Type = "New Type",
            Email = "new@email.com",
            Url = "http://new.com",
            Postal = "99999"
        };

        service.UpdateMarket(model);

        entity.Name.Should().Be("New Name");
        entity.Editor.Should().Be("New Editor");
        entity.Type.Should().Be("New Type");
        entity.Email.Should().Be("new@email.com");
        entity.Url.Should().Be("http://new.com");
        entity.Postal.Should().Be("99999");
        mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteMarket_RemovesEntityAndSaves()
    {
        var entity = new Market { MarketId = 1, Name = "To Be Deleted" };
        var mockSet = new Mock<DbSet<Market>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);
        mockContext.Setup(c => c.Markets.FirstOrDefault(m => m.MarketId == 1)).Returns(entity);

        var service = new MarketService(mockContext.Object);
        service.DeleteMarket(1);

        mockSet.Verify(m => m.Remove(entity), Times.Once);
        mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }
}