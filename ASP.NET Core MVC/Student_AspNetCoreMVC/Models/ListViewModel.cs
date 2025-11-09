using Microsoft.AspNetCore.Mvc.Rendering;

namespace Student_AspNetCoreMVC.Models
{
    public class ListViewModel
    {
        public string SelectedItem { get; set; } = null!;
        public IEnumerable<SelectListItem> Items { get; set; } = null!;

        public Course? Course { get; set; } = null!;
    }
}
