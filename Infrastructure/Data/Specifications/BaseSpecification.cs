using System.ComponentModel;
using System.Linq.Expressions;
using Core.Enums;
using Core.Specifications;

namespace Infrastructure.Data.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; } = _ => true;
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    //sort
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public OrderBy OrderByDirection { get; private set; } = Core.Enums.OrderBy.Ascending;
    //paging
    public int Take { get; private set; } = -1; //No paging for default
    public int Skip { get; private set; } = 1;
    public bool IsPagingEnabled { get; private set; } = false;

   

    // Method to add include statements
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    
    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression, OrderBy direction = Core.Enums.OrderBy.Ascending)
    {
        if (!Enum.IsDefined(typeof(OrderBy), direction))
            throw new InvalidEnumArgumentException(nameof(direction), (int)direction, typeof(OrderBy));
        OrderBy = orderByExpression;
        OrderByDirection = direction;
    }
    
    protected void ApplyPaging(int? skip, int? take)
    {
        Skip = skip??0;
        Take = take??10;
        IsPagingEnabled = true;
    }
    
}
