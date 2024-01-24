using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<ProductModel> GetProductByIdAsync(Guid id);
    Task<IList<ProductModel>> GetProductsAsync();
}