using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmicStarfrontWiki.Model
{
    public enum Category
    {
        Factions, 
        Characters, 
        Enemies, 
        Weapons, 
        Armour, 
        Missions, 
        Technology, 
        Vehicles, 
        Locations,
        Events,
        History
    }

    public class WikiPage
    {
        [Key]
        public int Id { get; set; }

        public required Category Category { get; set; }
        public required string Title { get; set; }
        public List<Section> Sections { get; set; } = new List<Section>();
        public string? Image { get; set; }
        public string? ImageStyle { get; set; }
        public Gallery? Gallery { get; set; }
        public int? Subcategory { get; set; }
    }

    public class Section
    {
        [Key]
        public int Id { get; set; } // Primary key

        public int Order { get; set; } // Order in which the section appears
        public required string Header { get; set; }
        public List<Content> Contents { get; set; } = new List<Content>();

        [ForeignKey(nameof(WikiPage))]
        public int WikiPageId { get; set; } // Foreign key to associate with WikiPage

        public required WikiPage WikiPage { get; set; } // Navigation property
    }

    public class Content
    {
        [Key]
        public int Id { get; set; } // Primary key

        public int Order { get; set; } // Order in which the content appears within the section
        public string? Subheader { get; set; }
        public required string Text { get; set; }

        [ForeignKey(nameof(Section))]
        public int SectionId { get; set; } // Foreign key to associate with Section

        public required Section Section { get; set; } // Navigation property
        public string? Image { get; set; }
        public string? ImageStyle { get; set; }
    }

    public class Gallery
    {
        [Key]
        public int Id { get; set; }

        public List<string>? Images { get; set; }
        public List<string>? ImageStyles { get; set; }
        public List<string>? Captions { get; set; }

        [ForeignKey(nameof(WikiPage))]
        public int WikiPageId { get; set; } // Foreign key to associate with WikiPage

        public required WikiPage WikiPage { get; set; } // Navigation property
    }
}
