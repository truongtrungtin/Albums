using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(ISpecification<T> spec);
    Task<IList<T>> ListAsync();
    Task<IList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}