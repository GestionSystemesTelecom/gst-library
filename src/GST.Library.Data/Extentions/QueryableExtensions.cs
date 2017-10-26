using System;
using System.Linq;
using System.Linq.Expressions;

namespace GST.Library.Data.Extentions
{
    /// <summary>
    /// QueryableExtensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Allow to do an OrderBy on a string list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortModel"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        /// Thanks to http://stackoverflow.com/questions/36298868/how-to-dynamically-order-by-certain-entity-properties-in-entityframework-7-core
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string[] sortModel, string sort = "asc")
        {

            if (sortModel == null 
                || sortModel.Length == 0 
                || string.IsNullOrEmpty(sortModel.First()) 
                || string.IsNullOrWhiteSpace(sortModel.First()))
            {
                return source;
            }

            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortModel)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, item);
                var method = string.Equals(sort, "desc", StringComparison.OrdinalIgnoreCase) ?
                (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));

                count++;
            }

            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}
