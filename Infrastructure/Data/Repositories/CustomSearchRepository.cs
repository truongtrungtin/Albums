using Infrastructure.Data.ViewModel;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class CustomSearchRepository
    {
        public static List<T> CustomSearchFunc<T>(DatatableViewModel model, out int filteredResultsCount, out int totalResultsCount, IQueryable<T> query, string sortByDefault, string strSortDir = null)
        {
            //var searchBy = (model.search != null) ? model.search.value : null;
            var searchBy = "";
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                var columnIndex = model.order[0].column;
                //default sort on the 1st column
                sortBy = model.columns[columnIndex].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
                if (sortByDefault != "STT" && sortBy == "STT")
                {
                    sortBy = "";
                    sortDir = (strSortDir == "asc" || strSortDir == null);
                }
            }

            var result = GetData<T>(query, searchBy, take, skip, sortBy, sortByDefault, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<T>();
            }
            return result;
        }

        public static List<T> GetData<T>(IQueryable<T> list, string searchBy, int take, int skip, string sortBy, string sortByDefault, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            if (String.IsNullOrEmpty(sortBy))
            {
                sortBy = sortByDefault;
                //sortDir = true;
            }

            var param = Expression.Parameter(typeof(T), "c");
            var body = sortBy.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            IOrderedQueryable<T> result;
            if (sortBy != "STT")
            {
                result = sortDir == true ?
                (IOrderedQueryable<T>)Queryable.OrderBy(list.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<T>)Queryable.OrderByDescending(list.AsQueryable(), (dynamic)Expression.Lambda(body, param));
            }
            else
            {
                result = (IOrderedQueryable<T>)list;
            }
            var a = result.ToList();

            List<T> res = result.Skip(skip).Take(take).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            filteredResultsCount = result.Count();
            totalResultsCount = res.Count();

            return res;
        }
    }
}
