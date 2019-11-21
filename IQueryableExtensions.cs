using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqHomework
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName)
        {
            var x = Expression.Parameter(source.ElementType, "x");
            var selector = Expression.Lambda(
                Expression.PropertyOrField(x, propertyName), x);

            return (IQueryable<TEntity>)source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "OrderBy",
                    new Type[] { source.ElementType, selector.Body.Type },
                    source.Expression,
                    selector));
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string propertyName, string text)
        {
            var x = Expression.Parameter(source.ElementType, "x");
            var selector = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    Expression.Call(
                        Expression.Call(
                            Expression.PropertyOrField(x, propertyName),
                            typeof(object).GetMethod("ToString", new Type[] { })),
                        typeof(string).GetMethod("ToLower", new Type[] { })),
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    Expression.Constant(text)), x);
            return source.Where(selector);
        }
    }
}
