using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.Services;

public static class FinePolicy
{
    public const decimal DailyRate = 1.50m;

    // These arguments are local Darwin calendar dates, not UTC event timestamps.
    public static decimal Calculate(DateTime dueDate, DateTime asOfDate) =>
        Math.Max(0, (asOfDate.Date - dueDate.Date).Days) * DailyRate;

    public static void RecordReturn(Loan loan, DateTime returnDate,
        bool payNow, DateTime paymentTimeUtc)
    {
        loan.FineAmount = Calculate(loan.DueDate, returnDate);
        loan.FinePaid = loan.FineAmount == 0 || payNow;
        loan.FinePaidDate = loan.FineAmount > 0 && payNow ? paymentTimeUtc : null;
    }
}
