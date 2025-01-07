using BlazorWebAssembly.Enums;

namespace BlazorWebAssembly.Models;

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
