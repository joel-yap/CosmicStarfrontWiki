using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.AccessControl;
using System.Text.Json;
using System.Xml.Linq;

namespace CosmicStarfrontWiki.API;

public static class WikiApi
{
    public static void MapWikiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        //var factionGroup = endpoints.MapGroup("/factions").WithTags("Factions");

        //factionGroup.MapGet("/names", GetFactionNames);
        //factionGroup.MapGet("/get{name}", GetFaction);
        //factionGroup.MapPut("/add", AddFaction);
        //factionGroup.MapPut("/edit{id}", EditFaction);

        var pageGroup = endpoints.MapGroup("/pages").WithTags("Pages");

        pageGroup.MapPut("/add", AddPage);
        pageGroup.MapGet("/get", GetPage);
        pageGroup.MapGet("/getall", GetPages);

        //var characterGroup = endpoints.MapGroup("/characters").WithTags("Characters");

        //characterGroup.MapGet("/names", GetCharacterNames);
        //characterGroup.MapGet("/get{name}", GetCharacter);
    }

    //public static IResult GetFactionNames()
    //{
    //    var factions = new List<Faction>();
    //    var factionNames = new List<string>();

    //    using (var context = new AppDbContext())
    //    {
    //        factions = context.Factions.ToList();
    //        foreach (var faction in factions)
    //        {
    //            //Console.WriteLine(faction.Name.ToString());
    //            factionNames.Add(faction.Name.ToString());
    //        }
    //    }

    //    var jsonFactionNames = JsonSerializer.Serialize(factionNames);

    //    return Results.Ok(jsonFactionNames);
    //}

    //public static IResult GetFaction(string name)
    //{
    //    var factions = new List<Faction>();

    //    using (var context = new AppDbContext())
    //    {
    //        factions = context.Factions.ToList();
    //    }

    //    var searchResult = factions.Where(f => f.Name == name).SingleOrDefault();

    //    if (searchResult != null)
    //    {
    //        // Make a DTO subset of the model to prevent users knowing exactly how the models are stored.
    //        var dto = new FactionDTO
    //        {
    //            Name = searchResult.Name,
    //            Description = searchResult.Description
    //        };

    //        var jsonFaction = JsonSerializer.Serialize(dto);

    //        return Results.Ok(jsonFaction);
    //    }
    //    else
    //    {
    //        return Results.BadRequest($"The faction {name} does not exist.");
    //    }
    //}

    //public static IResult AddFaction(FactionDTO faction)
    //{
    //    using (var context = new AppDbContext())
    //    {
    //        Faction newFaction = new Faction
    //        {
    //            Name = faction.Name,
    //            Description = faction.Description,
    //        };
    //        context.Add(newFaction);
    //        context.SaveChanges();
    //    }
    //    return Results.Ok();
    //}

    //public static IResult EditFaction(int id, FactionDTO updatedFaction)
    //{
    //    using (var context = new AppDbContext())
    //    {
    //        // Find the existing faction by Id
    //        var faction = context.Factions.FirstOrDefault(f => f.Id == id);
    //        if (faction == null)
    //        {
    //            return Results.NotFound(); // Faction not found
    //        }

    //        // Update the faction's properties
    //        faction.Name = updatedFaction.Name;
    //        faction.Description = updatedFaction.Description;

    //        // Save changes to the database
    //        context.SaveChanges();

    //        return Results.Ok(faction); // Return the updated faction
    //    }
    //}


    //public static IResult GetCharacterNames()
    //{
    //    var characterNames = new List<string>();

    //    /*
    //    for (var i = 0; i < 10; i++)
    //    {
    //        // Mimic fetching characters
    //        var character = new Character
    //        {
    //            Id = i,
    //            Name = string.Empty,
    //            Description = string.Empty,
    //            Stats = string.Empty,
    //        };

    //        // Make a DTO that does not have all of a character's metadata
    //        characterNames.Add(character.Name);
    //    }
    //    */
    //    characterNames.Add("Kane Cormac");
    //    characterNames.Add("Sigrid Andersen");
    //    characterNames.Add("Vorak Thar'khan");

    //    return Results.Ok(characterNames);
    //}

    //public static IResult GetCharacter(string name)
    //{
    //    var characters = new List<Character>();

    //    // Mock factions
    //    for (var i = 0; i < 10; i++)
    //    {
    //        var character = new Character
    //        {
    //            Id = i,
    //            Name = string.Empty,
    //            Description = string.Empty,
    //            Stats = string.Empty,
    //        };

    //        characters.Add(character);
    //    }

    //    var character2 = new Character
    //    {
    //        Id = 11,
    //        Name = "Kane Cormac",
    //        Description = string.Empty,
    //        Stats = string.Empty,
    //    };
    //    characters.Add(character2);

    //    var searchResult = characters.Where(f => f.Name == name).SingleOrDefault();

    //    if (searchResult != null)
    //    {
    //        // Make a DTO subset of the model to prevent users knowing exactly how the models are stored.
    //        var dto = new CharacterDTO
    //        {
    //            Name = searchResult.Name,
    //            Description = searchResult.Description,
    //            Stats = searchResult.Stats,
    //        };

    //        return Results.Ok(dto);
    //    }
    //    else
    //    {
    //        return Results.BadRequest($"The character {name} does not exist.");
    //    }
    //}

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
            var searchSections = sections.Where(s => s.WikiPageId == searchResult.Id).ToList();
            var resultSections = new List<SectionDTO>();
            foreach (var section in searchSections) 
            {
                var newSectionDTO = new SectionDTO
                {
                    Header = section.Header,
                    Order = section.Order,
                };
                resultSections.Add(newSectionDTO);
                var searchContents = contents.Where(c => c.SectionId == section.Id).ToList();
                var resultContents = new List<ContentDTO>();
                foreach (var content in searchContents)
                {
                    var newContentDTO = new ContentDTO
                    {
                        Text = content.Text,
                        Subheader = content.Subheader,
                        Order = content.Order,
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

            List<Section> sectionList = page.Sections.Select(s => new Section
            {
                Header = s.Header,
                WikiPageId = newPage.Id,
                WikiPage = newPage,
                Order = s.Order,
            }).ToList();

            context.Sections.AddRange(sectionList);

            context.SaveChanges();

            foreach (var section in newPage.Sections)
            {
                foreach (var sectionDTO in page.Sections)
                {
                    if (sectionDTO.Header == section.Header)
                    {
                        List<Content> contentList = new List<Content>();
                        foreach (var contentDTO in sectionDTO.Contents)
                        {
                            Content newContent = new Content
                            {
                                Order = contentDTO.Order,
                                Subheader = contentDTO.Subheader,
                                Text = contentDTO.Text,
                                Section = section
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
    public int Order { get; set; } // Order in which the section appears
    public required string Header { get; set; }
    public List<ContentDTO> Contents { get; set; } = new List<ContentDTO>();
}

public class ContentDTO
{
    public int Order { get; set; } // Order in which the content appears within the section
    public string? Subheader { get; set; }
    public required string Text { get; set; }
}


