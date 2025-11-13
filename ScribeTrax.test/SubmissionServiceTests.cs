using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using System.Collections.Generic;
using System.Linq;

public class SubmissionServiceTests
{
    private List<Submission> GetFakeSubmissions() => new()
    {
        new Submission
        {
            SubmissionId = 1,
            WorkId = 1,
            MarketId = 1,
            SubmissionDate = new DateTime(2025, 11, 1),
            Work = new Work
            {
                WorkId = 1,
                Title = "Teaser",
                Genre = new Genre { GenreId = 1, Name = "Fusion" },
                Byline = new Byline { BylineId = 1, Name = "Tommy Bolin" }
            },
            Market = new Market { MarketId = 1, Name = "Jazz Monthly", Type = "Print", Editor = "Jane" }
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
    public void GetSubmissionById_ReturnsEnrichedViewModel()
    {
        // Step 1: Hydrated test data
        var submissions = GetFakeSubmissions(); // includes Work, Market, Byline, Genre
        var payments = new List<Payment>();     // or include a matching Payment if needed

        // Step 2: Mock Submissions DbSet
        var queryableSubs = submissions.AsQueryable();
        var mockSubs = new Mock<DbSet<Submission>>();
        mockSubs.As<IQueryable<Submission>>().Setup(m => m.Provider).Returns(queryableSubs.Provider);
        mockSubs.As<IQueryable<Submission>>().Setup(m => m.Expression).Returns(queryableSubs.Expression);
        mockSubs.As<IQueryable<Submission>>().Setup(m => m.ElementType).Returns(queryableSubs.ElementType);
        mockSubs.As<IQueryable<Submission>>().Setup(m => m.GetEnumerator()).Returns(() => queryableSubs.GetEnumerator());

        // ✅ Step 3: Mock Payments DbSet — this is where we park it
        var queryablePayments = payments.AsQueryable();
        var mockPayments = new Mock<DbSet<Payment>>();
        mockPayments.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(queryablePayments.Provider);
        mockPayments.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(queryablePayments.Expression);
        mockPayments.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(queryablePayments.ElementType);
        mockPayments.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(() => queryablePayments.GetEnumerator());

        // Step 4: Mock context
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Submissions).Returns(mockSubs.Object);
        mockContext.Setup(c => c.Payments).Returns(mockPayments.Object); // ← parked here

        // Step 5: Run service
        var service = new SubmissionService(mockContext.Object);
        var result = service.GetSubmissionById(1);

        // Step 6: Assert
        result.Should().NotBeNull();
        result.WorkTitle.Should().Be("Teaser");
        result.BylineName.Should().Be("Tommy Bolin");
        result.MarketName.Should().Be("Jazz Monthly");
    }



    [Fact]
    public void CreateSubmission_AddsEntityAndSaves()
    {
        var mockSet = new Mock<DbSet<Submission>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Submissions).Returns(mockSet.Object);

        var service = new SubmissionService(mockContext.Object);
        var submission = new Submission { SubmissionId = 2, WorkId = 1, MarketId = 2 };

        service.CreateSubmission(submission);

        mockSet.Verify(m => m.Add(It.IsAny<Submission>()), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateSubmission_UpdatesEntityAndSaves()
    {
        var mockSet = new Mock<DbSet<Submission>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Submissions).Returns(mockSet.Object);

        var service = new SubmissionService(mockContext.Object);
        var submission = new Submission { SubmissionId = 1, WorkId = 1 };

        service.UpdateSubmission(submission);

        mockSet.Verify(m => m.Update(submission), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteSubmission_RemovesEntityAndSaves()
    {
        var entity = new Submission { SubmissionId = 1, WorkId = 1 };
        var mockSet = new Mock<DbSet<Submission>>();
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Submissions).Returns(mockSet.Object);
        mockContext.Setup(c => c.Submissions.Find(1)).Returns(entity);

        var service = new SubmissionService(mockContext.Object);
        service.DeleteSubmission(1);

        mockSet.Verify(m => m.Remove(entity), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }
}