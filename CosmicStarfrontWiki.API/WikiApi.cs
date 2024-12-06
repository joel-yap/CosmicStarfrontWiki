using CosmicStarfrontWiki.Data;
using CosmicStarfrontWiki.Model;
using System;
using System.Text.Json;
using System.Xml.Linq;

namespace CosmicStarfrontWiki.API;

public static class WikiApi
{
    public static void MapWikiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var factionGroup = endpoints.MapGroup("/factions").WithTags("Factions");

        factionGroup.MapGet("/names", GetFactionNames);
        factionGroup.MapGet("/get{name}", GetFaction);
        factionGroup.MapPut("/add", AddFaction);
        factionGroup.MapPut("/edit{id}", EditFaction);

        var characterGroup = endpoints.MapGroup("/characters").WithTags("Characters");

        characterGroup.MapGet("/names", GetCharacterNames);
        characterGroup.MapGet("/get{name}", GetCharacter);
    }

    public static IResult GetFactionNames()
    {
        var factions = new List<Faction>();
        var factionNames = new List<string>();

        using (var context = new AppDbContext())
        {
            factions = context.Factions.ToList();
            foreach (var faction in factions)
            {
                //Console.WriteLine(faction.Name.ToString());
                factionNames.Add(faction.Name.ToString());
            }
        }

        var jsonFactionNames = JsonSerializer.Serialize(factionNames);

        return Results.Ok(jsonFactionNames);
    }

    public static IResult GetFaction(string name)
    {
        var factions = new List<Faction>();

        using (var context = new AppDbContext())
        {
            factions = context.Factions.ToList();
        }

        var searchResult = factions.Where(f => f.Name == name).SingleOrDefault();

        if (searchResult != null)
        {
            // Make a DTO subset of the model to prevent users knowing exactly how the models are stored.
            var dto = new FactionDTO
            {
                Name = searchResult.Name,
                Description = searchResult.Description
            };

            var jsonFaction = JsonSerializer.Serialize(dto);

            return Results.Ok(jsonFaction);
        }
        else
        {
            return Results.BadRequest($"The faction {name} does not exist.");
        }
    }

    public static IResult AddFaction(FactionDTO faction)
    {
        using (var context = new AppDbContext())
        {
            int maxId = context.Factions.Max(p => p.Id) + 1;
            Faction newFaction = new Faction
            {
                Id = maxId,
                Name = faction.Name,
                Description = faction.Description,
            };
            context.Add(newFaction);
            context.SaveChanges();
        }
        return Results.Ok();
    }

    public static IResult EditFaction(int id, FactionDTO updatedFaction)
    {
        using (var context = new AppDbContext())
        {
            // Find the existing faction by Id
            var faction = context.Factions.FirstOrDefault(f => f.Id == id);
            if (faction == null)
            {
                return Results.NotFound(); // Faction not found
            }

            // Update the faction's properties
            faction.Name = updatedFaction.Name;
            faction.Description = updatedFaction.Description;

            // Save changes to the database
            context.SaveChanges();

            return Results.Ok(faction); // Return the updated faction
        }
    }


    public static IResult GetCharacterNames()
    {
        var characterNames = new List<string>();

        /*
        for (var i = 0; i < 10; i++)
        {
            // Mimic fetching characters
            var character = new Character
            {
                Id = i,
                Name = string.Empty,
                Description = string.Empty,
                Stats = string.Empty,
            };

            // Make a DTO that does not have all of a character's metadata
            characterNames.Add(character.Name);
        }
        */
        characterNames.Add("Kane Cormac");
        characterNames.Add("Sigrid Andersen");
        characterNames.Add("Vorak Thar'khan");

        return Results.Ok(characterNames);
    }

    public static IResult GetCharacter(string name)
    {
        var characters = new List<Character>();

        // Mock factions
        for (var i = 0; i < 10; i++)
        {
            var character = new Character
            {
                Id = i,
                Name = string.Empty,
                Description = string.Empty,
                Stats = string.Empty,
            };

            characters.Add(character);
        }

        var character2 = new Character
        {
            Id = 11,
            Name = "Kane Cormac",
            Description = string.Empty,
            Stats = string.Empty,
        };
        characters.Add(character2);

        var searchResult = characters.Where(f => f.Name == name).SingleOrDefault();

        if (searchResult != null)
        {
            // Make a DTO subset of the model to prevent users knowing exactly how the models are stored.
            var dto = new CharacterDTO
            {
                Name = searchResult.Name,
                Description = searchResult.Description,
                Stats = searchResult.Stats,
            };

            return Results.Ok(dto);
        }
        else
        {
            return Results.BadRequest($"The character {name} does not exist.");
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