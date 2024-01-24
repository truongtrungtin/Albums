namespace Core.Entities;

public class Product: BaseEntity
{
    public string? Name { get; set; }
    public double? Price { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public string? Location { get; set; }
    public double? size { get; set; }

    public TimeSpan? DuringTime { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? UploadTime { get; set; }
    public string? UserUpload { get; set; }


    // Foreign Key for ProductType
    public Guid? ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }

    // Foreign Key for ProductBrand
    public Guid? ProductBrandId { get; set; }
    public ProductBrand? ProductBrand { get; set; }
}
