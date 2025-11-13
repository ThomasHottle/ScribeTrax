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

public class PaymentServiceTest
{
    private List<Payment> GetFakePayments() => new()
    {
        new Payment
        {
            PaymentId = 1,
            WorkId = 1,
            MarketId = 1,
            PaymentDate = new DateOnly(2025, 11, 1),
            PaymentTypeId = 1,
            Work = new Work { WorkId = 1, Title = "Teaser" },
            Market = new Market { MarketId = 1, Name = "Jazz Monthly" },
            PaymentType = new PaymentType { PaymentTypeId = 1, Name = "Advance" }
        },
        new Payment
        {
            PaymentId = 2,
            WorkId = 2,
            MarketId = 2,
            PaymentDate = new DateOnly(2025, 11, 2),
            PaymentTypeId = 2,
            Work = new Work { WorkId = 2, Title = "Spectrum" },
            Market = new Market { MarketId = 2, Name = "Fusion Weekly" },
            PaymentType = new PaymentType { PaymentTypeId = 2, Name = "Royalty" }
        }
    };

    private Mock<DbSet<Payment>> GetMockPaymentSet(List<Payment> data)
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<Payment>>();
        mockSet.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return mockSet;
    }

    [Fact]
    public void GetAllPayments_ReturnsHydratedViewModels()
    {
        var mockSet = GetMockPaymentSet(GetFakePayments());
        var mockContext = new Mock<ScribeTraxDbContext>();
        mockContext.Setup(c => c.Payments).Returns(mockSet.Object);

        var service = new PaymentService(mockContext.Object);
        var result = service.GetAllPayments().ToList();

        result.Should().HaveCount(2);
        result[0].WorkTitle.Should().Be("Teaser");
        result[0].MarketName.Should().Be("Jazz Monthly");
        result[0].PaymentTypeName.Should().Be("Advance");

        result[1].WorkTitle.Should().Be("Spectrum");
        result[1].MarketName.Should().Be("Fusion Weekly");
        result[1].PaymentTypeName.Should().Be("Royalty");
    }
}