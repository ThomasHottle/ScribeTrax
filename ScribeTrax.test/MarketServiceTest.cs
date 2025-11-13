using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using ScribeTrax.ViewModels;
using Xunit;

public class MarketServiceTests
{
    private List<Market> GetFakeMarkets() => new()
    {
        new Market
        {
            MarketId = 1,
            Name = "Jazz Monthly",
            Type = "Print",
            Editor = "Jane",
            Email = "jane@jazzmonthly.com",
            Url = "http://jazzmonthly.com",
            Postal = "12345"
        },
        new Market
        {
            MarketId = 2,
            Name = "Fusion Weekly",
            Type = "Web",
            Editor = "Bob",
            Email = "bob@fusionweekly.com",
            Url = "http://fusionweekly.com",
            Postal = "67890"
        }
    };

    private Mock<DbSet<Market>> GetMockMarketSet(List<Market> data)
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<Market>>();
        mockSet.As<IQueryable<Market>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Market>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Market>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Market>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return mockSet;
    }

    [Fact]
    public void GetAllMarkets_ReturnsAllMarkets()
    {
        var mockSet = GetMockMarketSet(GetFakeMarkets());
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);

        var service = new MarketService(mockContext.Object);
        var result = service.GetAllMarkets().ToList();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Fusion Weekly"); // Alphabetical order
        result[1].Name.Should().Be("Jazz Monthly");
    }

    [Fact]
    public void GetMarketById_ReturnsCorrectMarket()
    {
        var mockSet = GetMockMarketSet(GetFakeMarkets());
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);

        var service = new MarketService(mockContext.Object);
        var result = service.GetMarketById(1);

        result.Should().NotBeNull();
        result.Name.Should().Be("Jazz Monthly");
        result.Editor.Should().Be("Jane");
    }

    [Fact]
    public void UpdateMarket_UpdatesFieldsAndSaves()
    {
        var market = GetFakeMarkets().First();
        var mockSet = GetMockMarketSet(new List<Market> { market });
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);

        var service = new MarketService(mockContext.Object);
        var updated = new MarketViewModel
        {
            MarketId = 1,
            Name = "Jazz Monthly Updated",
            Editor = "Janet",
            Type = "Web",
            Email = "janet@updated.com",
            Url = "http://updated.com",
            Postal = "99999"
        };

        service.UpdateMarket(updated);

        market.Name.Should().Be("Jazz Monthly Updated");
        market.Editor.Should().Be("Janet");
        mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteMarket_RemovesMarketAndSaves()
    {
        var market = GetFakeMarkets().First();
        var mockSet = GetMockMarketSet(new List<Market> { market });
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Markets).Returns(mockSet.Object);

        var service = new MarketService(mockContext.Object);
        service.DeleteMarket(1);

        mockSet.Verify(m => m.Remove(It.Is<Market>(mk => mk.MarketId == 1)), Times.Once);
        mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }
}