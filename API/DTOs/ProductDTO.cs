namespace API.DTOs;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public string ProductType { get; set; }
    public string ProductBrand { get; set; }
    public string Location { get; set; }
    public double Size { get; set; }
    public TimeSpan DuringTime { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UploadTime { get; set; }
    public string UserUpload { get; set; }
    public int? MonthCreate => CreateTime.Month;
    public int? YearCreate => CreateTime.Year;

    // Other properties as required
}


