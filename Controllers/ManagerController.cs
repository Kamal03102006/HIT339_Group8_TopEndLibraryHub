using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow.Date;

            var viewModel = new ManagerDashboardViewModel
            {
                // Item statistics
                TotalItems = await _context.Items.CountAsync(),
                AvailableItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Available),
                BorrowedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Borrowed),
                DamagedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Damaged),
                DestroyedItems = await _context.Items.CountAsync(
                    item => item.Status == ItemStatus.Destroyed),

                // Loan statistics
                TotalLoans = await _context.Loans.CountAsync(),
                ActiveLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate == null),
                OverdueLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate == null &&
                            loan.DueDate < today),
                ReturnedLoans = await _context.Loans.CountAsync(
                    loan => loan.ReturnedDate != null),

                // Fine statistics
                TotalFines = await _context.Loans
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m,

                PaidFines = await _context.Loans
                    .Where(loan => loan.FinePaid)
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m,

                OutstandingFines = await _context.Loans
                    .Where(loan => loan.FineAmount > 0 && !loan.FinePaid)
                    .SumAsync(loan => (decimal?)loan.FineAmount) ?? 0m
            };

            viewModel.AvailabilityRate = viewModel.TotalItems == 0
                ? 0
                : (double)viewModel.AvailableItems /
                  viewModel.TotalItems * 100;

            viewModel.ItemsByType =
            [
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Books",
                    TotalItems = await _context.Books.CountAsync(),
                    AvailableItems = await _context.Books.CountAsync(
                        item => item.Status == ItemStatus.Available)
                },
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Music",
                    TotalItems = await _context.MusicItems.CountAsync(),
                    AvailableItems = await _context.MusicItems.CountAsync(
                        item => item.Status == ItemStatus.Available)
                },
                new ItemTypeStatisticViewModel
                {
                    ItemType = "Toys",
                    TotalItems = await _context.Toys.CountAsync(),
                    AvailableItems = await _context.Toys.CountAsync(
                        item => item.Status == ItemStatus.Available)
                }
            ];

            viewModel.PopularItems = await _context.Loans
                .AsNoTracking()
                .GroupBy(loan => new
                {
                    loan.ItemId,
                    loan.Item.LibraryCode,
                    loan.Item.Name
                })
                .Select(group => new PopularItemViewModel
                {
                    ItemId = group.Key.ItemId,
                    LibraryCode = group.Key.LibraryCode,
                    Name = group.Key.Name,
                    LoanCount = group.Count()
                })
                .OrderByDescending(item => item.LoanCount)
                .ThenBy(item => item.Name)
                .Take(5)
                .ToListAsync();

            viewModel.RecentLoans = await _context.Loans
                .AsNoTracking()
                .OrderByDescending(loan => loan.BorrowedDate)
                .ThenByDescending(loan => loan.Id)
                .Take(5)
                .Select(loan => new RecentLoanViewModel
                {
                    LoanId = loan.Id,
                    LibraryCode = loan.Item.LibraryCode,
                    ItemName = loan.Item.Name,
                    BorrowerName = loan.Borrower.FullName,
                    BorrowedDate = loan.BorrowedDate,
                    DueDate = loan.DueDate,
                    ReturnedDate = loan.ReturnedDate,
                    FineAmount = loan.FineAmount,
                    FinePaid = loan.FinePaid
                })
                .ToListAsync();

            return View(viewModel);
        }
    }
}