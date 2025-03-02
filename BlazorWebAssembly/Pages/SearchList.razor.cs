using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages
{
    public partial class SearchList
    {
        [Parameter]
        public string SearchQuery { get; set; } = string.Empty;
        public List<WikiPageDTO> Result = [];

        [Inject]
        private PageService PageService { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            Result = await PageService.SearchPages(SearchQuery);
        }

        private string TruncateText(string text, int maxLength)
        {
            return (string.IsNullOrEmpty(text) || (text.Length <= maxLength)) 
                ? text 
                : $"{text[..maxLength]}...";
        }
    }
}