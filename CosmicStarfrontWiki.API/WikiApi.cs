using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Security.AccessControl;
using System.Text.Json;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using Section = CosmicStarfrontWiki.Model.Section;

namespace CosmicStarfrontWiki.API;

public static class WikiApi
{
    public static void MapWikiEndpoints(this IEndpointRouteBuilder endpoints)
    {

        var pageGroup = endpoints.MapGroup("/pages").WithTags("Pages");

        pageGroup.MapPut("/add", AddPage);
        pageGroup.MapGet("/get", GetPage);
        pageGroup.MapGet("/getall", GetPages);
        pageGroup.MapPut("/editpage", EditPage);
        pageGroup.MapPut("/editsection", EditSection);
        pageGroup.MapPut("/editcontent", EditContent);

    }

    public static IResult GetPage(string name)
    {
        var pages = new List<WikiPage>();
        var sections = new List<Section>();
        var contents = new List<Content>();

        using (var context = new AppDbContext())
        {
            pages = context.WikiPages.ToList();
            sections = context.Sections.ToList();
            contents = context.Contents.ToList();
        }

        var searchResult = pages.Where(f => f.Title == name).SingleOrDefault();

        if (searchResult != null)
        {
            var result = new WikiPageDTO
            {
                Category = searchResult.Category,
                Title = searchResult.Title,
            };
            var searchSections = sections.Where(s => s.WikiPageId == searchResult.Id).OrderBy(s => s.Order).ToList();
            var resultSections = new List<SectionDTO>();
            foreach (var section in searchSections) 
            {
                var newSectionDTO = new SectionDTO
                {
                    Header = section.Header,
                };
                resultSections.Add(newSectionDTO);
                var searchContents = contents.Where(c => c.SectionId == section.Id).OrderBy(c => c.Order).ToList();
                var resultContents = new List<ContentDTO>();
                foreach (var content in searchContents)
                {
                    var newContentDTO = new ContentDTO
                    {
                        Text = content.Text,
                        Subheader = content.Subheader,
                    };
                    resultContents.Add(newContentDTO);
                }
                newSectionDTO.Contents = resultContents;
            }
            result.Sections = resultSections;

            return Results.Ok(result);
        }
        else
        {
            return Results.BadRequest($"The page {name} does not exist.");
        }
    }

    public static IResult AddPage(WikiPageDTO page)
    {
        WikiPage newPage = new WikiPage
        {
            Category = page.Category,
            Title = page.Title
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
                                Order = contentOrder++
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


    public static IResult GetPages()
    {
        var pages = new List<WikiPage>();

        using (var context = new AppDbContext())
        {
            pages = context.WikiPages.ToList();
        }
        return Results.Ok(pages);

    }

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
        using (var context = new AppDbContext())
        {
            var sections = context.Sections.ToList();
            var searchResult = sections.Where(f => f.WikiPageId == pageID && f.Order == order).SingleOrDefault();
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
}


