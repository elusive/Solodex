using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;

namespace Elusive.Solodex.Data
{
    /// <summary>Defines a QueryFilter</summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class QueryFilter<T> : IQueryFilter<T>
        where T : PersistableModel<T>
    {
        /// <summary>Initializes a new instance of the <see cref="QueryFilter{T}" /> class.</summary>
        public QueryFilter()
        {
            Filters = new List<Expression<Func<T, bool>>>();
            SortOrder = new List<Tuple<Expression, SortDirection>>();
            Includes = new List<Expression>();
        }

        /// <summary>Gets or sets Filters.</summary>
        private List<Expression<Func<T, bool>>> Filters { get; set; }

        /// <summary>Gets or sets Includes.</summary>
        private List<Expression> Includes { get; set; }

        /// <summary>Gets or sets SortOrder.</summary>
        private List<Tuple<Expression, SortDirection>> SortOrder { get; set; }

        /// <summary>Gets or sets zero-based FirstRecordDesired.</summary>
        public int? FirstRecordDesired { get; set; }

        /// <summary>Gets or sets RecordsAvailable.  This will be set when the query is executed.</summary>
        public int? RecordsAvailable { get; set; }

        /// <summary>
        ///     Gets or sets the number of records desired.  Setting to 0 will retrieve just the record count
        ///     RecordsAvailable.
        /// </summary>
        public int? RecordsDesired { get; set; }

        /// <summary>Adds a filter condition for the query</summary>
        /// <param name="predicate">The predicate.</param>
        public IQueryFilter<T> AddFilter(Expression<Func<T, bool>> predicate)
        {
            Filters.Add(predicate);
            return this;
        }

        /// <summary>Adds a sort expression for the query</summary>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="direction">The direction.</param>
        /// <typeparam name="T2">The type</typeparam>
        public IQueryFilter<T> AddSort<T2>(
            Expression<Func<T, T2>> sortExpression,
            SortDirection direction = SortDirection.Ascending)
        {
            SortOrder.Add(new Tuple<Expression, SortDirection>(sortExpression, direction));
            return this;
        }

        /// <summary>Retrieves the filters ordered by priority</summary>
        /// <returns>The filters</returns>
        public IEnumerable<Expression<Func<T, bool>>> GetFilterExpressions()
        {
            return Filters;
        }

        /// <summary>Retrieves the include expressions</summary>
        /// <returns>The include expressions</returns>
        public IEnumerable<Expression> GetIncludeExpressions()
        {
            return Includes;
        }

        /// <summary>Retrieves the sort expressions ordered by priority</summary>
        /// <returns>The sort expressions</returns>
        public IEnumerable<Tuple<Expression, SortDirection>> GetSortExpressions()
        {
            return SortOrder;
        }

        /// <summary>Adds an include (eager load) expression</summary>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="T2">The type</typeparam>
        public IQueryFilter<T> Include<T2>(Expression<Func<T, T2>> expression)
        {
            Includes.Add(expression);
            return this;
        }
        /// <summary>
        /// Selects the specified expression.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public IQueryFilter<T, T2> Select<T2>(Expression<Func<T, T2>> expression)
        {
            var qf = new QueryFilter<T, T2>();
            qf.SortOrder = SortOrder.ToList();
            qf.Includes = Includes.ToList();
            qf.Filters = Filters.ToList();
            qf.Select(expression);
            return qf;
        }
    }

    /// <summary>
    /// QueryFilter supporting Select clause
    /// </summary>
    /// <typeparam name="T">Source Type</typeparam>
    /// <typeparam name="T2">Result Type</typeparam>
    public class QueryFilter<T, T2> : QueryFilter<T>, IQueryFilter<T, T2>
        where T : PersistableModel<T>
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private bool _applyDistinct;

        /// <summary>Gets or sets Includes.</summary>
        private Expression SelectExpression { get; set; }

        /// <summary>
        /// Ensures results are distinct.
        /// </summary>
        /// <remarks>MUST be used with Select()</remarks>
        public IQueryFilter<T, T2> Distinct()
        {
            if (SelectExpression == null)
            {
                throw new InvalidOperationException("Distinct can only be used with Select.");
            }
            _applyDistinct = true;
            return this;
        }

        /// <summary>Retrieves the include expressions</summary>
        /// <returns>The include expressions</returns>
        public Expression GetSelectExpression()
        {
            return SelectExpression;
        }

        /// <summary>Defines the select expression</summary>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="T2">The type</typeparam>
        public IQueryFilter<T, T2> Select(Expression<Func<T, T2>> expression)
        {
            if (SelectExpression != null)
            {
                throw new InvalidOperationException("Only a single select clause may be used.");
            }
            SelectExpression = expression;
            return this;
        }

        /// <summary>
        /// Determines if a distinct filter should be applied.
        /// </summary>
        /// <returns>True if a distinct filter should be applied.</returns>
        public bool ShouldApplyDistinct()
        {
            return _applyDistinct;
        }
    }
}