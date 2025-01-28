using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
