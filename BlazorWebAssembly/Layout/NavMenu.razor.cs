using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Layout
{
    public partial class NavMenu
    {
        private bool _collapseNavMenu = true;

        private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        [Inject]
        private IHttpClientFactory HttpClientFactory { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }

        private SearchModel _searchModel = new SearchModel();

        private async Task HandleSearch()
        {
            var client = HttpClientFactory.CreateClient("MyApi");
            if (string.IsNullOrWhiteSpace(_searchModel.Query))
            {
                return;
            }
            var result = await client.GetAsync($"https://localhost:7002/pages/{_searchModel.Query}");

            if (result.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo($"/wiki/{_searchModel.Query}");
            }
            else
            { // Handle the case where no result is found 
              //NavigationManager.NavigateTo("/not-found"); 
                NavigationManager.NavigateTo($"/Searchlist/{_searchModel.Query}");
            }
        }

        public class SearchModel { public string? Query { get; set; } }
    }
}