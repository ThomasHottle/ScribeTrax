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
        var submissions = GetFakeSubmissions();
        var payments = GetFakePayments();

        var mockSubs = GetMockSet(submissions);
        var mockPays = GetMockSet(payments);

        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Submissions).Returns(mockSubs.Object);
        mockContext.Setup(c => c.Payments).Returns(mockPays.Object);
        mockContext.Setup(c => c.Submissions
            .Include(It.IsAny<string>())).Returns(mockSubs.Object);

        mockContext.Setup(c => c.Submissions.FirstOrDefault(s => s.SubmissionId == 1))
                   .Returns(submissions.First());

        var service = new SubmissionService(mockContext.Object);
        var result = service.GetSubmissionById(1);

        result.Should().NotBeNull();
        result.WorkTitle.Should().Be("Te