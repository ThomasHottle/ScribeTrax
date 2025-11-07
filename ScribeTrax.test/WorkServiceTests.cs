using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using System.Collections.Generic;
using System.Linq;

public class WorkServiceTests
{
    private List<Work> GetFakeWorks() => new()
    {
        new Work
        {
            WorkId = 1,
            Title = "Teaser",
            Type = "Album",
            BylineId = 1,
            GenreId = 1,
            Byline = new Byline { BylineId = 1, Name = "Tommy Bolin" },
            Genre = new Genre { GenreId = 1, Name = "Fusion" }
        }
    };

    private List<Submission> GetFakeSubmissions() => new()
    {
        new Submission
        {
            SubmissionId = 1,
            WorkId = 1,
            SubmissionDate = new DateTime(2025, 11, 1),
            Market = new Market { MarketId = 1, Name = "Jazz Monthly" }
        }
    };

    private List<Payment> GetFakePayments() => new()
    {
        new Payment { PaymentId = 1, WorkId = 1 }
    };

    private Mock<DbSet<T>> GetMockSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockSet;
    }

    [Fact]
    public void GetWorkById_ReturnsEnrichedViewModel()
    {
        var works = GetFakeWorks();
        var submissions = GetFakeSubmissions();
        var payments = GetFakePayments();

        var mockWorks = GetMockSet(works);
        var mockSubs = GetMockSet(submissions);
        var mockPays = GetMockSet(payments);

        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Works).Returns(mockWorks.Object);
        mockContext.Setup(c => c.Submissions).Returns(mockSubs.Object);
        mockContext.Setup(c => c.Payments).Returns(mockPays.Object);

        var service = new WorkService(mockContext.Object);
        var result = service.GetWorkById(1);

        result.Should().NotBeNull();
        result.Title.Should().Be("Teaser");
        result.BylineName.Should().Be("Tommy Bolin");
        result.GenreName.Should().Be("Fusion");
        result.SubmissionCount.Should().Be(1);
        result.HasPayments.Should().BeTrue();
        result.LastSubmittedDate.Should().Be(new DateTime(2025, 11, 1));
        result.MostRecentMarketName.Should().Be("Jazz Monthly");
    }

    [Fact]
    public void CreateWork_AddsEntityAndSaves()
    {
        var mockSet = new Mock<DbSet<Work>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Works).Returns(mockSet.Object);

        var service = new WorkService(mockContext.Object);
        var work = new Work { Title = "Wild Dogs", Type = "Track" };

        service.CreateWork(work);

        mockSet.Verify(m => m.Add(It.IsAny<Work>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateWork_UpdatesEntityAndSaves()
    {
        var mockSet = new Mock<DbSet<Work>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Works).Returns(mockSet.Object);

        var service = new WorkService(mockContext.Object);
        var work = new Work { WorkId = 1, Title = "Updated Title" };

        service.UpdateWork(work);

        mockSet.Verify(m => m.Update(work), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteWork_RemovesEntityAndSaves()
    {
        var entity = new Work { WorkId = 1, Title = "To Be Deleted" };
        var mockSet = new Mock<DbSet<Work>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Works).Returns(mockSet.Object);
        mockContext.Setup(c => c.Works.Find(1)).Returns(entity);

        var service = new WorkService(mockContext.Object);
        service.DeleteWork(1);

        mockSet.Verify(m => m.Remove(entity), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}