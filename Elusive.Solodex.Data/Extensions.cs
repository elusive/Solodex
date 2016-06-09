using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Elusive.Solodex.Core.Enumerations;

namespace Elusive.Solodex.Data
{
    /// <summary>Extension methods for DAL</summary>
    public static class Extensions
    {
        /// <summary>.Net 4.0 Core System Assembly</summary>
        private const string CoreAssembly =
            "System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

        /// <summary>Entity Framework Assembly</summary>
        private const string EfAssembly =
            "EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

        /// <summary>Includes additional relations for eager loading</summary>
        /// <param name="query">The query.</param>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The query</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression expression)
        {
            return ApplyMethod<T, IQueryable<T>>(query, expression, "Include", EfAssembly);
        }

        /// <summary>Performs ordering for a query</summary>
        /// <param name="query">The query.</param>
        /// <param name="sortExp">The sort exp.</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The ordered query</returns>
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> query,
            Tuple<Expression, SortDirection> sortExp)
        {
            return ApplyMethod<T, IOrderedQueryable<T>>(
                query,
                sortExp.Item1,
                sortExp.Item2 == SortDirection.Ascending ? "OrderBy" : "OrderByDescending",
                CoreAssembly);
        }

        /// <summary>LINQ Select</summary>
        /// <param name="query">The query.</param>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="T">The source type</typeparam>
        /// <typeparam name="T2">The result type</typeparam>
        /// <returns>The query</returns>
        public static IQueryable<T2> Select<T, T2>(this IQueryable<T> query, Expression expression)
        {
            if (expression == null)
            {
                return query as IQueryable<T2>;
            }
            return ApplyMethod<T, IQueryable<T2>>(query, expression, "Select", CoreAssembly);
        }

        /// <summary>Performs additional sorting</summary>
        /// <param name="query">The query.</param>
        /// <param name="sorts">The sorts.</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The ordered query</returns>
        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> query,
            IEnumerable<Tuple<Expression, SortDirection>> sorts)
        {
            foreach (var s in sorts)
            {
                query = ApplyMethod<T, IOrderedQueryable<T>>(
                    query,
                    s.Item1,
                    s.Item2 == SortDirection.Ascending ? "ThenBy" : "ThenByDescending",
                    CoreAssembly);
            }
            return query;
        }

        /// <summary>Applies the where func if the predicate is true</summary>
        /// <param name="query">The query.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="func">The func.</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The query</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool predicate, Expression<Func<T, bool>> func)
        {
            return predicate ? query.Where(func) : query;
        }

        /// <summary>Reflectively calls the specified method based on the runtime types</summary>
        /// <param name="query">The query.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="assembly">The assembly.</param>
        /// <typeparam name="T">The type</typeparam>
        /// <typeparam name="TReturnType">The return type</typeparam>
        /// <returns>The query</returns>
        private static TReturnType ApplyMethod<T, TReturnType>(
            IQueryable<T> query,
            Expression expression,
            string methodName,
            string assembly)
        {
            var body = expression.GetType().GetProperty("Body").GetValue(expression, null);
            var expressionType = (Type) body.GetType().GetProperty("Type").GetValue(body, null);

            var mi = GetExtensionMethods(Assembly.Load(assembly), methodName, true).First();
            MethodInfo method = mi.MakeGenericMethod(typeof (T), expressionType);
            return (TReturnType) method.Invoke(null, new object[] {query, expression});
        }

        /// <summary>Reflectively queries for extension methods</summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="secondArgumentIsGeneric">The second argument is generic.</param>
        /// <returns>Enumerable list of matching methods</returns>
        private static IEnumerable<MethodInfo> GetExtensionMethods(
            Assembly assembly,
            string methodName,
            bool secondArgumentIsGeneric)
        {
            var query = from type in assembly.GetTypes()
                where type.IsSealed && !type.IsGenericType && !type.IsNested
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                where method.IsDefined(typeof (ExtensionAttribute), false)
                where
                    method.Name == methodName && method.GetParameters()[0].ParameterType.Name.Contains("Queryable")
                    && // Filter out IEnumerable & IEnumerable<T> extensions
                    method.GetParameters()[0].ParameterType.IsGenericType
                // Filter out IQueryable (non-generic) overloads
                where method.GetParameters()[1].ParameterType.IsGenericType == secondArgumentIsGeneric
                select method;
            return query;
        }
    }
}