
using Xunit;

namespace TopEndLibraryHub.Tests;

public class FineCalculationTests
{
    private const decimal DailyFineRate = 1.50m;

    [Theory]
    [InlineData(0, 0.00)]
    [InlineData(1, 1.50)]
    [InlineData(4, 6.00)]
    [InlineData(8, 12.00)]
    public void FineAmount_MatchesExpectedRate(
        int overdueDays,
        double expectedFine)
    {
        decimal fine = overdueDays * DailyFineRate;

        Assert.Equal((decimal)expectedFine, fine);
    }

    [Theory]
    [InlineData(6.00, false, false)]
    [InlineData(6.00, true, true)]
    [InlineData(0.00, false, true)]
    public void FinePaymentStatus_MatchesExpectedResult(
        double fineAmount,
        bool payFineNow,
        bool expectedPaid)
    {
        decimal fine = (decimal)fineAmount;

        bool finePaid = fine == 0 || payFineNow;

        Assert.Equal(expectedPaid, finePaid);
    }
}
