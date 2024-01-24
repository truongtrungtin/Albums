namespace Infrastructure.Data.ViewModel
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Excerpt { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Sku { get; set; }
        public string partNumber { get; set; }
        public string url { get; set; }
        public decimal? Price { get; set; }
        public decimal? ComparePrice { get; set; }
        public List<string> Images { get; set; }
        public List<string> Badges { get; set; }
        public decimal? Rating { get; set; }
        public decimal? Reviews { get; set; }
        public string Availability { get; set; }
        public object Compatibility { get; set; }
        public string BrandCode { get; set; }
        public Guid? CategoryId { get; set; }
        public BrandViewModel Brand { get; set; }
        public List<string> Tags { get; set; }
        public ProductTypeViewModel Type { get; set; }
        public ShopCategoryViewModel Categories { get; set; }
        public List<ProductAttributeViewModel> Attributes { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
