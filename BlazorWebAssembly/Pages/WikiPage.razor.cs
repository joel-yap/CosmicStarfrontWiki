using BlazorWebAssembly.Services;

namespace BlazorWebAssembly.Pages
{
    public partial class WikiPage
    {
        public string? ResultPage;
        protected override async Task OnInitializedAsync()
        {
            var page = await PageService.GetWikiPage();
            ResultPage = page;
        }

        //protected override
    }
}