using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;

namespace BlazorWebAssembly.Pages
{
    public partial class Characters
    {
        public List<WikiPageDTO>? ResultCategory;
        public required Category Category { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            ResultCategory = await PageService.GetPagesByCategory(Category.Character);
        }
    }
}