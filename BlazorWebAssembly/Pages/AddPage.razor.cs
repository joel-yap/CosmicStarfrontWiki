using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWebAssembly.Pages
{
    public partial class AddPage
    {
        private string _notificationMessage = "";

        [Inject]
        private IHttpClientFactory HttpClientFactory { get; set; } = default!;

        private WikiPageDTO wikiPage = new()
        {
            Category = Category.Characters, // Set a default or initial value 
            Title = "Initial Title" // Set a default or initial value 
        };

        private void NotifyUser()
        {
            _notificationMessage = "Page Created!";
        }

        private async Task HandleValidSubmit()
        {
            var client = HttpClientFactory.CreateClient("MyApi");
            await client.PostAsJsonAsync("https://localhost:7002/pages/", wikiPage);
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