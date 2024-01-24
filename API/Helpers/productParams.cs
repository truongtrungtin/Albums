namespace API.Helpers;

public class ProductParams
{
    public string? Sort { get; set; }
    public Guid? ProductTypeId { get; set; }
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string Search { get; set; } = "";

    public int GetSkip()
    {
        return (Page - 1) * PageSize;
    }
}