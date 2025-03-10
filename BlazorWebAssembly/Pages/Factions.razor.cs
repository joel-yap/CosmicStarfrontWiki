using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages
{
    public partial class Factions
    {
        public List<WikiPageDTO>? ResultCategory;
        public required Category Category { get; set; }

        [Inject]
        private PageService PageService { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            ResultCategory = await PageService.GetPagesByCategory(Category.Factions);
        }
    }
}