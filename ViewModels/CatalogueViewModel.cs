using TopEndLibraryHub.Models;

namespace TopEndLibraryHub.ViewModels
{
    public class CatalogueViewModel
    {
        public string SearchString { get; set; } = string.Empty;

        public string SelectedType { get; set; } = string.Empty;

        public string SelectedStatus { get; set; } = string.Empty;

        public List<Item> Items { get; set; } = new();

        public int ResultCount => Items.Count;

        public int AvailableCount =>
            Items.Count(item => item.Status == ItemStatus.Available);
    }
}