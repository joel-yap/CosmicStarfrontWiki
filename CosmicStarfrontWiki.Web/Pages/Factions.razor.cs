using CosmicStarfrontWiki.Web.Enums;
using CosmicStarfrontWiki.Web.Models;
using CosmicStarfrontWiki.Web.Services;
using Microsoft.AspNetCore.Components;

namespace CosmicStarfrontWiki.Web.Pages
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