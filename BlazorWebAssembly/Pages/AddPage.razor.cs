using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using System.Net.Http.Json;

namespace BlazorWebAssembly.Pages
{
    public partial class AddPage
    {
        private WikiPageDTO wikiPage = new WikiPageDTO
        {
            Category = Category.Character, // Set a default or initial value 
            Title = "Initial Title" // Set a default or initial value 
        };

        private async Task HandleValidSubmit()
        {
            await HttpClient.PutAsJsonAsync("https://localhost:7002/pages/add", wikiPage);
            // Handle the response if needed
        }

        private void AddSection()
        {
            wikiPage.Sections.Add(new SectionDTO { Header = "New Section" });
        }

        private void AddContent(SectionDTO section)
        {
            section.Contents.Add(new ContentDTO { Text = "New Content" });
        }
    }
}