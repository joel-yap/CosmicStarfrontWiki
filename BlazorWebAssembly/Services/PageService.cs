using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using System.Text.Json;
using System.Xml.Linq;

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

    public static async Task<List<string>> GetPagesByCategory(Category category)
    {
        HttpClient client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7002/pages/getpagescate?category={category}");
        response.EnsureSuccessStatusCode();

        var categoryList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<string>>(categoryList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }
}
