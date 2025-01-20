using BlazorWebAssembly.Enums;
using BlazorWebAssembly.Models;
using BlazorWebAssembly.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAssembly.Pages;

public partial class WikiPage
{
    public WikiPageDTO? ResultPage;
    public List<string>? ResultCategory;
    [Parameter]
    public required string Name {  get; set; }
    public required Category Category { get; set; }
    int sectionCounter = 0;
    protected override async Task OnParametersSetAsync()
    {
        var page = await PageService.GetWikiPage(Name);
        var category = await PageService.GetPagesByCategory(page.Category);
        ResultPage = page;
        ResultCategory = category;
        sectionCounter = 0;
    }
}