namespace Core.Entities
{
    public class BasketItem
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
            
        // Foreign Key for Product
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        //public Product Product { get; set; }
    }
}

