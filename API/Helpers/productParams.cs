namespace API.Helpers;

public class FileAttachmentParams
{
    public string? Sort { get; set; }
    public string? FileExtention { get; set; }
    public string? FileType { get; set; }
    public string? profile { get; set; }
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string Search { get; set; } = "";

    public int GetSkip()
    {
        return (Page - 1) * PageSize;
    }
}