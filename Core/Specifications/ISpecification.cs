using System.Linq.Expressions;
using Core.Enums;

namespace Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    OrderBy OrderByDirection { get; }
    
    //paging
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
    
}