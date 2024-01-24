using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private EntityDataContext _context;

        public Repository(EntityDataContext context)
        {
            _context = context;
        }

        //public async Task<T?> GetByIdAsync(Guid id)
        //{
        //    return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        //}

        public async Task<T?> GetByIdAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IList<T>> ListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _context.Set<T>().AsQueryable();

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);
            
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            
            if (spec.OrderBy != null)
            {
                query = spec.OrderByDirection == OrderBy.Ascending ?
                    query.OrderBy(spec.OrderBy) :
                    query.OrderByDescending(spec.OrderBy);
            }
            
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            return query;
        }

    }
}
