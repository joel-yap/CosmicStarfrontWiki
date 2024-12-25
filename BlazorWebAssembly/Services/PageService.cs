namespace BlazorWebAssembly.Services;

public class PageService
{
    //http client, make get call
    //add model folder for DTOs
    public static async Task<string> GetWikiPage()
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync("https://localhost:7002/pages/get?name=NCA");
        response.EnsureSuccessStatusCode();
        var wikiPage = await response.Content.ReadAsStringAsync();
        return wikiPage;
    }
}
