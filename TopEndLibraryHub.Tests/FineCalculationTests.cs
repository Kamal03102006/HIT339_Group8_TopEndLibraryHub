using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;

namespace TopEndLibraryHub.Tests;

public class FineCalculationTests
{
    [Theory]
    [InlineData(-1, 0.00)]
    [InlineData(0, 0.00)]
    [InlineData(1, 1.50)]
    [InlineData(4, 6.00)]
    [InlineData(8, 12.00)]
    public void ProductionFinePolicy_UsesCalendarDays(int days, double expected)
    {
        var due = new DateTime(2026, 10, 10);
        Assert.Equal((decimal)expected, FinePolicy.Calculate(due, due.AddDays(days).AddHours(23)));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReturnFine_RecordsPaymentStateAndTimestamp(bool payNow)
    {
        var paymentTime = new DateTime(2026, 10, 16, 1, 0, 0, DateTimeKind.Utc);
        var loan = new Loan { DueDate = new(2026, 10, 12) };
        FinePolicy.RecordReturn(loan, new(2026, 10, 16), payNow, paymentTime);
        Assert.Equal(6m, loan.FineAmount);
        Assert.Equal(payNow, loan.FinePaid);
        Assert.Equal(payNow ? paymentTime : (DateTime?)null, loan.FinePaidDate);
    }

    [Fact]
    public void NoFine_CannotRetainAnOldPaymentTimestamp()
    {
        var loan = new Loan { DueDate = new(2026, 10, 16), FineAmount = 6m,
            FinePaidDate = DateTime.UtcNow.AddDays(-1) };
        FinePolicy.RecordReturn(loan, new(2026, 10, 16), false, DateTime.UtcNow);
        Assert.Equal(0m, loan.FineAmount); Assert.True(loan.FinePaid); Assert.Null(loan.FinePaidDate);
    }

    [Fact]
    public void DarwinDate_ChangesAt1430Utc()
    {
        var time = new FixedClock { Now = DateTimeOffset.Parse("2026-10-15T14:29:59Z") };
        var clock = new LibraryClock(time);
        Assert.Equal(new DateTime(2026, 10, 15), clock.Today);
        time.Now = time.Now.AddSeconds(1);
        Assert.Equal(new DateTime(2026, 10, 16), clock.Today);
    }
}
