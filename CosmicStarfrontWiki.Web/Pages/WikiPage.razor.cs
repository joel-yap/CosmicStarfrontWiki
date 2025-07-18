using CosmicStarfrontWiki.Web.Enums;
using CosmicStarfrontWiki.Web.Models;
using CosmicStarfrontWiki.Web.Services;
using Microsoft.AspNetCore.Components;

namespace CosmicStarfrontWiki.Web.Pages;

public partial class WikiPage
{
    public WikiPageDTO? ResultPage;
    public List<string>? ResultCategory;

    [Parameter]
    public required string Name {  get; set; }

    [Parameter]
    public required Category Category { get; set; }

    [Inject]
    private PageService PageService { get; set; } = default!;

    int sectionCounter = 0;

    protected override async Task OnParametersSetAsync()
    {
        var page = await PageService.GetWikiPage(Name);
        var category = await PageService.GetPageTitlesByCategory(page.Category);
        ResultPage = page;
        ResultCategory = category;
        sectionCounter = 0;
    }
}