using Core.Entities;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EntityDataContext _context;

    public ProductRepository(EntityDataContext context)
    {
        _context = context;
    }

    public async Task<ProductModel> GetProductByIdAsync(Guid id)
    {
        var product = await _context.ProductModel
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        return product;
    }


    public async Task<IList<ProductModel>> GetProductsAsync()
    {
        return await _context.ProductModel
            .ToListAsync();
    }
}