using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly EntityDataContext _context;

        public GenericRepository(EntityDataContext context)
        {
            _context = context;
        }

        public async Task<T> GetProductByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new NotFoundException($"{typeof(T).Name} not found");
            }
            return entity;
        }

        public async Task<IList<T>> GetProductsAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IList<T>> GetProductBrandsAsync()
        {
            // This method assumes that T can be ProductBrand.
            // If T is not ProductBrand, this will throw an exception.
            if (typeof(T) == typeof(ProductBrand))
            {
                return await _context.Set<T>().ToListAsync();
            }
            throw new InvalidOperationException("Invalid entity type for GetProductBrandsAsync");
        }

        public async Task<IList<T>> GetProductTypesAsync()
        {
            // This method assumes that T can be ProductType.
            // If T is not ProductType, this will throw an exception.
            if (typeof(T) == typeof(ProductType))
            {
                return await _context.Set<T>().ToListAsync();
            }
            throw new InvalidOperationException("Invalid entity type for GetProductTypesAsync");
        }
    }
}