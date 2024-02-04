namespace API.Helpers;

public class FileAttachmentParams
{
    public string? Sort { get; set; }
    public string? FileExtention { get; set; }
    public string? FileType { get; set; }
    public Guid? profile { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string Search { get; set; } = "";

    public int GetSkip()
    {
        return (Page - 1) * PageSize;
    }
}