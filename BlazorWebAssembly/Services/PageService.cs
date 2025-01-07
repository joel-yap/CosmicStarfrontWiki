using BlazorWebAssembly.Models;
using System.Text.Json;

namespace BlazorWebAssembly.Services;

public class PageService
{
    //http client, make get call
    //add model folder for DTOs
    public static async Task<WikiPageDTO> GetWikiPage(string name)
    {
        HttpClient client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7002/pages/get?name={name}");
        response.EnsureSuccessStatusCode();

        var wikiPage = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<WikiPageDTO>(wikiPage, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }
}
