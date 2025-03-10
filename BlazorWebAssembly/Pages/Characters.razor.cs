using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages
{
    public partial class Characters
    {
        public List<WikiPageDTO>? ResultCategory;
        private const string TITLE = "Characters";
        public required Category Category { get; set; }

        [Inject]
        private PageService PageService { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            ResultCategory = await PageService.GetPagesByCategory(Category.Characters);
        }
    }
}