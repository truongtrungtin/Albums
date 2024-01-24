using Core.Entities;

namespace Infrastructure.Data.Specifications
{
    public class ProductCountSpecification : BaseSpecification<ProductModel>
    {
        public ProductCountSpecification(string? sort, Guid? productTypeId, Guid? productBrandId, string search)
        {
            // Apply filter criteria similar to ProductWithTypesAndBrandSpecification
            if (productTypeId.HasValue || productBrandId.HasValue || !string.IsNullOrWhiteSpace(search))
            {
                ApplyCriteria(x => 
                    (string.IsNullOrWhiteSpace(search) || x.ProductName.ToLower().Contains(search.ToLower()))
                );
            }
        }

        public ProductCountSpecification(string? sort, Guid? productTypeId)
        {
            // Apply filter criteria similar to ProductWithTypesAndBrandSpecification
            if (productTypeId.HasValue)
            {
                ApplyCriteria(x => x.CategoryId == productTypeId );
            }
        }
    }
}