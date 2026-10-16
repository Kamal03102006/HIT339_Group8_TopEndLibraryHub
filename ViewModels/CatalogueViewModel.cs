using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels
{
    public class CatalogueViewModel
    {
        public string SearchString { get; set; } = string.Empty;

        public string SelectedType { get; set; } = string.Empty;

        public string SelectedStatus { get; set; } = string.Empty;

        public List<Item> Items { get; set; } = new();
        public HashSet<int> AvailableItemIds { get; set; } = new();
        public HashSet<int> ReservedItemIds { get; set; } = new();

        public int ResultCount => Items.Count;

        public int AvailableCount => Items.Count(item => AvailableItemIds.Contains(item.Id));

        public string AvailabilityLabel(Item item) =>
            AvailableItemIds.Contains(item.Id) ? "Available" :
            item.Status == ItemStatus.InTransit ? "In transit" :
            ReservedItemIds.Contains(item.Id) && item.Status == ItemStatus.Available ? "On hold" :
            item.Status == ItemStatus.Available ? "Unavailable" : item.Status.ToString();
    }
}
