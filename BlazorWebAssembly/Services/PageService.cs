using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using BlazorWebAssembly.Pages;
using System.Text.Json;
using System.Xml.Linq;

namespace BlazorWebAssembly.Services;

public class PageService
{
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

    public static async Task<List<string>> GetPageTitlesByCategory(Category category)
    {
        HttpClient client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7002/pages/getpagetitlescateg?category={category}");
        response.EnsureSuccessStatusCode();

        var categoryList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<string>>(categoryList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }

    public static async Task<List<WikiPageDTO>> GetPagesByCategory(Category category)
    {
        HttpClient client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7002/pages/getpagescateg?category={category}");
        response.EnsureSuccessStatusCode();

        var categoryList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<WikiPageDTO>>(categoryList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }

    public static async Task<List<PanelDTO>> GetPanels(int page)
    {
        HttpClient client = new HttpClient();

        var response = await client.GetAsync($"https://localhost:7002/pages/getpanels?page={page}");
        response.EnsureSuccessStatusCode();

        var panelList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<PanelDTO>>(panelList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }
}
