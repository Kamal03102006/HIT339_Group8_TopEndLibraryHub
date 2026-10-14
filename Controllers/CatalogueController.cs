using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopEndLibraryHub.Data;
using TopEndLibraryHub.Models;
using TopEndLibraryHub.Services;
using TopEndLibraryHub.ViewModels;

namespace TopEndLibraryHub.Controllers
{
    [AllowAnonymous]
    public class CatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchString,
            string? itemType,
            string? status)
        {
            var search = searchString?.Trim() ?? string.Empty;
            if (search.Length > 100) search = search[..100];
            var selectedType = itemType?.Trim() ?? string.Empty;
            var selectedStatus = status?.Trim() ?? string.Empty;

            IQueryable<Item> query = _context.Items.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(item =>
                    item.LibraryCode.Contains(search) ||
                    item.Name.Contains(search) ||
                    item.Description.Contains(search) ||

                    (item is Book &&
                        (((Book)item).Author.Contains(search) ||
                         ((Book)item).BookGenre.Contains(search))) ||

                    (item is Music &&
                        (((Music)item).Artist.Contains(search) ||
                         ((Music)item).MusicGenre.Contains(search))) ||

                    (item is Toy &&
                        (((Toy)item).ToyType.Contains(search) ||
                         ((Toy)item).RecommendedAge.Contains(search) ||
                         (((Toy)item).Manufacturer ?? string.Empty).Contains(search))));
            }

            switch (selectedType.ToLowerInvariant())
            {
                case "book":
                    query = query.OfType<Book>();
                    selectedType = "Book";
                    break;

                case "music":
                    query = query.OfType<Music>();
                    selectedType = "Music";
                    break;

                case "toy":
                    query = query.OfType<Toy>();
                    selectedType = "Toy";
                    break;

                default:
                    selectedType = string.Empty;
                    break;
            }

            if (selectedStatus.Equals("OnHold", StringComparison.OrdinalIgnoreCase))
            {
                var onHoldIds = CirculationRules.ReservedItemIds(_context);
                query = query.Where(item => onHoldIds.Contains(item.Id));
                selectedStatus = "OnHold";
            }
            else if (Enum.TryParse<ItemStatus>(
                selectedStatus,
                true,
                out var parsedStatus) && Enum.IsDefined(parsedStatus))
            {
                query = parsedStatus == ItemStatus.Available ?
                    CirculationRules.GenerallyAvailable(_context, query) :
                    query.Where(item => item.Status == parsedStatus);
                selectedStatus = parsedStatus.ToString();
            }
            else
            {
                selectedStatus = string.Empty;
            }

            var availableIds = CirculationRules.GenerallyAvailable(_context).Select(item => item.Id);
            var reservedIds = CirculationRules.ReservedItemIds(_context);
            var items = await query.Include(item => item.Branch)
                .OrderBy(item => availableIds.Contains(item.Id) ? 0 : 1)
                .ThenBy(item => item.Name)
                .ToListAsync();
            var resultIds = items.Select(item => item.Id).ToArray();

            var viewModel = new CatalogueViewModel
            {
                SearchString = search,
                SelectedType = selectedType,
                SelectedStatus = selectedStatus,
                Items = items,
                AvailableItemIds = (await availableIds.Where(id => resultIds.Contains(id)).ToListAsync()).ToHashSet(),
                ReservedItemIds = (await reservedIds.Where(id => resultIds.Contains(id)).ToListAsync()).ToHashSet()
            };

            return View(viewModel);
        }
    }
}
