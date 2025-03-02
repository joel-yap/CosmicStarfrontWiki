using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CosmicStarfrontWiki.API;

public static class WikiApi
{
    public static void MapWikiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var pageGroup = endpoints.MapGroup("/pages").WithTags("Pages");

        pageGroup.MapPost("/", AddPage);
        pageGroup.MapGet("/{name}", GetPage);
        pageGroup.MapGet("/", GetPages);
        pageGroup.MapGet("/title", GetPageTitles);
        pageGroup.MapGet("/search/{name}", SearchPages);

        //pageGroup.MapPut("/editpage", EditPage);
        //pageGroup.MapPut("/editsection", EditSection);
        //pageGroup.MapPut("/editcontent", EditContent);
        //pageGroup.MapPut("/editgallery", EditGallery);
        //pageGroup.MapPut("/addpanel", AddPanel);
        pageGroup.MapGet("/panel", GetPanels);
    }

    public static IResult GetPage(string name)
    {
        using var context = new AppDbContext();

        var wikiPage = context.WikiPages
            .Include(x => x.Sections)
            .ThenInclude(y => y.Contents)
            .Include(x => x.Gallery)
            .ToList()
            .Where(x => x.Title == name)
            .SingleOrDefault();

        if (wikiPage != null) 
        {
            return Results.Ok(CreateWikiPageDTO(wikiPage));
        }
        else
        {
            return Results.BadRequest($"The page {name} does not exist.");
        }
    }

    public static IResult GetPageTitles(Category? category)
    {
        using var context = new AppDbContext();

        var pages = new List<WikiPage>();
        var results = new List<string>();

        if (category != null) 
        {
            pages = context.WikiPages
                .Where(x => x.Category == category)
                .ToList();
        } else
        {
            pages = context.WikiPages
                .ToList();
        }
        
        if (pages != null)
        {
            foreach (var page in pages)
            {
                results.Add(page.Title);
            }
        }
        return Results.Ok(results);
    }

    public static IResult SearchPages(string name)
    {
        using var context = new AppDbContext();

        var wikiPages = context.WikiPages
            .Where(x => x.Title.ToUpper().Contains(name.ToUpper()))
            .Include(x => x.Sections)
            .ThenInclude(y => y.Contents)
            .Include(x => x.Gallery)
            .ToList();

        var results = new List<WikiPageDTO>();

        if (wikiPages.Count > 0)
        {
            foreach (var page in wikiPages)
            {
                results.Add(CreateWikiPageDTO(page));
            }
            return Results.Ok(results);
        }
        else
        {
            return Results.BadRequest("No matching pages.");
        }
    }

    public static IResult AddPage(WikiPageDTO page)
    {
        WikiPage newPage = new WikiPage
        {
            Category = page.Category,
            Title = page.Title,
            Image = page.Image,
            ImageStyle = page.ImageStyle,
        };
        using (var context = new AppDbContext())
        {
            context.Add(newPage);
            context.SaveChanges();

            int sectionOrder = 0;

            List<Section> sectionList = page.Sections.Select(s => new Section
            {
                Header = s.Header,
                WikiPageId = newPage.Id,
                WikiPage = newPage,
                Order = sectionOrder++
            }).ToList();

            context.Sections.AddRange(sectionList);

            context.SaveChanges();

            int contentOrder = 0;

            foreach (var section in newPage.Sections)
            {
                foreach (var sectionDTO in page.Sections)
                {
                    if (sectionDTO.Header == section.Header)
                    {
                        contentOrder = 0;
                        List<Content> contentList = new List<Content>();
                        foreach (var contentDTO in sectionDTO.Contents)
                        {
                            Content newContent = new Content
                            {
                                Subheader = contentDTO.Subheader,
                                Text = contentDTO.Text,
                                Section = section,
                                Order = contentOrder++,
                                Image = contentDTO.Image,
                                ImageStyle = contentDTO.ImageStyle,
                            };
                            contentList.Add(newContent);
                        }
                        section.Contents = contentList;
                    }
                }
            }

            context.SaveChanges();
        }
        return Results.Ok();
    }

    //public static IResult AddPanel(PanelDTO panel)
    //{
    //    PagePanel newPanel = new PagePanel();
    //    newPanel.PageId = panel.PageId;
    //    newPanel.BoxId = panel.Box;
    //    newPanel.PanelName = panel.Name;
    //    newPanel.Image = panel.Image;
    //    newPanel.Link = panel.Link;
    //    using (var context = new AppDbContext())
    //    {
    //        context.Add(newPanel);
    //        context.SaveChanges();
    //    }
    //    return Results.Ok();
    //}

    public static IResult GetPanels(int page)
    {
        var panels = new List<PagePanel>();
        using (var context = new AppDbContext()) 
        {
            panels = context.PagePanels.ToList();
        }
        var panels2 = panels.Where(p => p.PageId == page).ToList();
        var results = new List<PanelDTO>();
        foreach(var panel in panels2)
        {
            var result = new PanelDTO();
            result.PageId = panel.PageId;
            result.Box = panel.BoxId;
            result.Image = panel.Image;
            result.Link = panel.Link;
            result.Name = panel.PanelName;
            results.Add(result);
        }
        return Results.Ok(results);
    }

    public static IResult GetPages(Category? category)
    {
        var pages = new List<WikiPage>();
        var results = new List<WikiPageDTO>();

        using (var context = new AppDbContext())
        {
            if (category != null)
            {
                pages = context.WikiPages.Where(x => x.Category == category)
                    .Include(x => x.Sections)
                    .ThenInclude(y => y.Contents)
                    .Include(x => x.Gallery)
                    .ToList();
            }
            else
            {
                pages = context.WikiPages
                .Include(x => x.Sections)
                .ThenInclude(y => y.Contents)
                .Include(x => x.Gallery)
                .ToList();
            }

            if (pages != null)
            {
                foreach (var page in pages)
                {
                    results.Add(CreateWikiPageDTO(page));
                }
            }
        }
        return Results.Ok(results);
    }

    public static WikiPageDTO CreateWikiPageDTO(WikiPage page)
    {
        var result = new WikiPageDTO
        {
            Category = page.Category,
            Title = page.Title,
            Image = page.Image,
            ImageStyle = page.ImageStyle,
            Sections = page.Sections
                .OrderBy(x => x.Order)
                .Select(x =>
                    new SectionDTO
                    {
                        Header = x.Header,
                        Contents = x.Contents
                            .OrderBy(y => y.Order)
                            .Select(y =>
                                new ContentDTO
                                {
                                    Text = y.Text,
                                    Subheader = y.Subheader,
                                    Image = y.Image,
                                    ImageStyle = y.ImageStyle,
                                }
                            )
                            .ToList()
                    }
                )
                .ToList(),
            Gallery = page.Gallery != null
                ? new GalleryDTO
                {
                    Images = page.Gallery.Images,
                    ImageStyles = page.Gallery.ImageStyles,
                    Captions = page.Gallery.Captions,
                }
                : null
        };
        return result;
    }

    //public static IResult GetPageTitlesByCategory(Category category)
    //{
    //    var pages = new List<WikiPage>();

    //    using (var context = new AppDbContext())
    //    {
    //        pages = context.WikiPages.ToList();
    //    }
    //    var searchResult = pages.Where(f => f.Category == category).ToList();
    //    List<string> result = new List<string>();
    //    foreach(var page in searchResult)
    //    {
    //        result.Add(page.Title);
    //    }
    //    return Results.Ok(result);
    //}

    //public static IResult GetPagesByCategory(Category category)
    //{
    //    var pages = new List<WikiPage>();
    //    var results = new List<WikiPageDTO>();
    //    var sections = new List<Section>();
    //    var contents = new List<Content>();
    //    var galleries = new List<Gallery>();

    //    using (var context = new AppDbContext())
    //    {
    //        pages = context.WikiPages.ToList();
    //        sections = context.Sections.ToList();
    //        contents = context.Contents.ToList();
    //        galleries = context.Galleries.ToList();
    //    }
    //    var searchResult = pages.Where(f => f.Category == category).ToList();

    //    foreach (var page in searchResult)
    //    {
    //        var result = new WikiPageDTO
    //        {
    //            Category = page.Category,
    //            Title = page.Title,
    //            Image = page.Image,
    //            ImageStyle = page.ImageStyle,
    //        };
    //        var searchSections = sections.Where(s => s.WikiPageId == page.Id).OrderBy(s => s.Order).ToList();
    //        var resultSections = new List<SectionDTO>();
    //        foreach (var section in searchSections)
    //        {
    //            var newSectionDTO = new SectionDTO
    //            {
    //                Header = section.Header,
    //            };
    //            resultSections.Add(newSectionDTO);
    //            var searchContents = contents.Where(c => c.SectionId == section.Id).OrderBy(c => c.Order).ToList();
    //            var resultContents = new List<ContentDTO>();
    //            foreach (var content in searchContents)
    //            {
    //                var newContentDTO = new ContentDTO
    //                {
    //                    Text = content.Text,
    //                    Subheader = content.Subheader,
    //                    Image = content.Image,
    //                    ImageStyle = content.ImageStyle,
    //                };
    //                resultContents.Add(newContentDTO);
    //            }
    //            newSectionDTO.Contents = resultContents;
    //        }
    //        result.Sections = resultSections;
    //        var searchGalleries = galleries.Where(g => g.WikiPageId == page.Id).SingleOrDefault();
    //        if (searchGalleries != null)
    //        {
    //            var resultGallery = new GalleryDTO
    //            {
    //                Images = searchGalleries.Images,
    //                ImageStyles = searchGalleries.ImageStyles,
    //                Captions = searchGalleries.Captions,
    //            };
    //            result.Gallery = resultGallery;
    //        }
    //        results.Add(result);
    //    }
    //    return Results.Ok(results);
    //}

    //All four Edit methods not implemented yet
    public static IResult EditPage(string title, string newTitle)
    {
        using (var context = new AppDbContext())
        {
            var wikiPages = context.WikiPages.ToList();
            var searchResult = wikiPages.Where(f => f.Title == title).SingleOrDefault();
            if (searchResult != null)
            {
                searchResult.Title = newTitle;
                context.SaveChanges();
                return Results.Ok(searchResult.Title);
            }
            else
            {
                return Results.NotFound();
            }
        }
    }

    public static IResult EditSection(int pageID, int order, string newHeader)
    {
        using var context = new AppDbContext();

        var sections = context.Sections.ToList();
        var searchResult = sections
            .Where(x => x.WikiPageId == pageID)
            .Where(x => x.Order == order)
            .SingleOrDefault();

        if (searchResult != null)
        {
            searchResult.Header = newHeader;
            context.SaveChanges();

            return Results.Ok(searchResult.Header);
        }
        else
        {
            return Results.NotFound();
        }
    }

    public static IResult EditContent(int sectionID, int order, ContentDTO newSection)
    {
        using (var context = new AppDbContext())
        {
            var contents = context.Contents.ToList();
            var searchResult = contents.Where(f => f.SectionId == sectionID && f.Order == order).SingleOrDefault();
            if (searchResult != null)
            {
                searchResult.Subheader = newSection.Subheader;
                searchResult.Text = newSection.Text;
                context.SaveChanges();
                return Results.Ok();
            }
            else
            {
                return Results.NotFound();
            }
        }
    }

    public static IResult EditGallery(string name, GalleryDTO galleryDTO)
    {
        using (var context = new AppDbContext())
        {
            var pages = context.WikiPages.ToList();
            var galleries = context.Galleries.ToList();
            var searchResult = pages.Where(f => f.Title == name).SingleOrDefault();
            if (searchResult != null)
            {
                var galleryResult = galleries.Where(g => g.WikiPageId == searchResult.Id).SingleOrDefault();
                if (galleryResult != null) { 
                    // Update the existing Gallery
                    galleryResult.Images = galleryDTO.Images;
                    galleryResult.ImageStyles = galleryDTO.ImageStyles;
                    galleryResult.Captions = galleryDTO.Captions; 
                } else
                {
                    searchResult.Gallery = new Gallery
                    {
                        WikiPage = searchResult,
                        Images = galleryDTO.Images,
                        ImageStyles = galleryDTO.ImageStyles,
                        Captions = galleryDTO.Captions,
                    };
                }
                context.SaveChanges();
                return Results.Ok();
            }
            else
            {
                return Results.NotFound();
            }
        }
    }
}

public class FactionDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class CharacterDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Stats { get; set; }
}

public class WikiPageDTO
{
    //public int Id { get; set; }
    public required Category Category { get; set; }
    public required string Title { get; set; }
    public List<SectionDTO> Sections { get; set; } = new List<SectionDTO>();
    public string? Image { get; set; }
    public string? ImageStyle { get; set; }
    public GalleryDTO? Gallery { get; set; }
}

public class SectionDTO
{
    public required string Header { get; set; }
    public List<ContentDTO> Contents { get; set; } = new List<ContentDTO>();
}

public class ContentDTO
{
    public string? Subheader { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }
    public string? ImageStyle { get; set; }
}

public class GalleryDTO
{
    public List<string>? Images { get; set; }
    public List<string>? ImageStyles { get; set; }
    public List<string>? Captions { get; set; }
}

public class PanelDTO
{
    public int? PageId { get; set; }
    public int? Box {  get; set; }
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
}


