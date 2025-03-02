using System.ComponentModel.DataAnnotations;

namespace CosmicStarfrontWiki.Model
{
    public class PagePanel
    {
        [Key]
        public int Id { get; set; }
        public int? PageId { get; set; }
        public int? BoxId { get; set; }
        public string? PanelName { get; set; }
        public string? Image {  get; set; }
        public string? Link { get; set; }
    }
}
