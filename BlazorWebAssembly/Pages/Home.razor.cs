using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages
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