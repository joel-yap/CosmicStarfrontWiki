using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using System.Text.Json;

namespace BlazorWebAssembly.Services;

public class PageService
{
    public PageService(IHttpClientFactory factory)
    {
        _httpClientFactory = factory;
    }

    private IHttpClientFactory _httpClientFactory;

    public async Task<WikiPageDTO> GetWikiPage(string name)
    {
        var client = _httpClientFactory.CreateClient("MyApi");

        var response = await client.GetAsync($"https://localhost:7002/pages/{name}");
        response.EnsureSuccessStatusCode();

        var wikiPage = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<WikiPageDTO>(wikiPage, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        ArgumentNullException.ThrowIfNull(dto);
        return dto;
    }

    public async Task<List<WikiPageDTO>> SearchPages(string name)
    {
        var client = _httpClientFactory.CreateClient("MyApi");

        var response = await client.GetAsync($"https://localhost:7002/pages/search/{name}");
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return [];
        }

        var wikiPages = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<WikiPageDTO>>(wikiPages, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return dto ?? [];
    }

    public async Task<List<string>> GetPageTitlesByCategory(Category category)
    {
        var client = _httpClientFactory.CreateClient("MyApi");

        var response = await client.GetAsync($"https://localhost:7002/pages/title?category={category}");
        response.EnsureSuccessStatusCode();

        var categoryList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<string>>(categoryList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }

    public async Task<List<WikiPageDTO>> GetPagesByCategory(Category category)
    {
        var client = _httpClientFactory.CreateClient("MyApi");

        var response = await client.GetAsync($"https://localhost:7002/pages/?category={category}");
        response.EnsureSuccessStatusCode();

        var categoryList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<WikiPageDTO>>(categoryList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }

    public async Task<List<PanelDTO>> GetPanels(int page)
    {
        var client = _httpClientFactory.CreateClient("MyApi");

        var response = await client.GetAsync($"https://localhost:7002/pages/panel?page={page}");
        response.EnsureSuccessStatusCode();

        var panelList = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<List<PanelDTO>>(panelList, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(dto);

        return dto;
    }
}
