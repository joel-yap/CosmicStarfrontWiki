using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CosmicStarfrontWiki.Model
{
    public enum Category
    {
        Faction, 
        Character, 
        Enemy, 
        Weapon, 
        Armour, 
        Mission, 
        Technology, 
        Vehicle, 
        Location,
        Event,
        History
    }
    public class WikiPage
    {
        [Key]
        public int Id { get; set; }
        public required Category Category { get; set; }
        public required string Title { get; set; }
        public List<Section> Sections { get; set; } = new List<Section>();
    }

    public class Section
    {
        [Key]
        public int Id { get; set; } // Primary key
        public int Order { get; set; } // Order in which the section appears
        public required string Header { get; set; }
        public List<Content> Contents { get; set; } = new List<Content>();
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
        public int SectionId { get; set; } // Foreign key to associate with Section
        public required Section Section { get; set; } // Navigation property
    }
}
