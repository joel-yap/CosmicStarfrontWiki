using CosmicStarfrontWiki.Web.Models;
using CosmicStarfrontWiki.Web.Services;
using Microsoft.AspNetCore.Components;

namespace CosmicStarfrontWiki.Web.Pages
{
    public partial class Home
    {
        public List<PanelDTO>? Panels;
        private const string HOME_TITLE = "Cosmic Starfront Wiki";

        [Inject]
        private PageService PageService { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            Panels = await PageService.GetPanels(0);
        }
    }
}