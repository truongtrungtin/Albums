namespace API.DTOs
{
    public class ProductTypeCreateDTO
    {
        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }
        }
        public string Name { get; set; }

        // Additional properties specific to creating a product type, if any
    }
}
