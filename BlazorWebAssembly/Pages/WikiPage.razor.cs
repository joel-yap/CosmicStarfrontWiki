using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages;

public partial class WikiPage
{
    public WikiPageDTO? ResultPage;
    [Parameter]
    public required string Name {  get; set; }
    protected override async Task OnInitializedAsync()
    {
        var page = await PageService.GetWikiPage(Name);
        ResultPage = page;
    }
}