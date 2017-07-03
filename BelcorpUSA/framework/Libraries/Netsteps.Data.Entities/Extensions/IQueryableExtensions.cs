using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetSteps.Common.Base;
using NetSteps.Common.Expressions;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Expressions;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: IQueryable Extensions
	/// Created: 07-21-2010
	/// </summary>
	public static class IQueryableExtensions
	{
		public static IQueryable<T> ApplyOrderByFilters<T, TKey>(this IQueryable<T> items, PaginatedListParameters parameters, Expression<Func<T, TKey>> defaultOrderBy, ObjectContext context)
		{
			if (!parameters.OrderBy.IsNullOrEmpty())
				items = items.ApplyOrderByFilter(parameters, context);
			else if (defaultOrderBy != null)
			{
				if (parameters.OrderByDirection == Constants.SortDirection.Ascending)
					items = items.OrderBy(defaultOrderBy);
				else
					items = items.OrderByDescending(defaultOrderBy);
			}

			return items;
		}
        public static IQueryable<T> ApplyOrderByFilter<T, TKey>(this IQueryable<T> items, NetSteps.Common.Constants.SortDirection sortDirection, Expression<Func<T, TKey>> defaultOrderBy)
		{
			if (defaultOrderBy != null)
			{
				if (sortDirection == Constants.SortDirection.Ascending)
					items = items.OrderBy(defaultOrderBy);
				else
					items = items.OrderByDescending(defaultOrderBy);
			}

			return items;
		}
		public static IQueryable<T> ApplyOrderByFilter<T>(this IQueryable<T> items, PaginatedListParameters parameters, ObjectContext context, bool checkTranslations = true)
		{
			IOrderedQueryable<T> sorted = null;
			if (!parameters.OrderBy.IsNullOrEmpty())
			{
				Type t = typeof(T);
				if (t.PropertyExists(parameters.OrderBy))
				{
					if (parameters.OrderBy.EndsWith("TermName"))
					{
						var expression = items.MakeTermTranslationExpression(parameters.OrderBy, parameters.LanguageID, context);
						sorted = parameters.OrderByDirection == Constants.SortDirection.Ascending ? items.OrderBy(expression) : items.OrderByDescending(expression);
					}
					else
					{
						sorted = parameters.OrderByDirection == Constants.SortDirection.Ascending ? items.OrderBy(parameters.OrderBy) : items.OrderByDescending(parameters.OrderBy);
					}
				}
				else if (checkTranslations)
				{
					Type type = typeof(T);
					if (parameters.OrderBy.Contains("."))
					{
						string prop = parameters.OrderBy.Substring(0, parameters.OrderBy.LastIndexOf('.'));
						PropertyInfo info;
						foreach (string p in prop.Split('.'))
						{
							info = type.GetPropertyCached(p);
							if (info != null)
							{
								type = info.PropertyType;
							}
							else
								throw new ArgumentException(string.Format("Property '{0}' does not exist on type '{1}'", p, type.Name));
						}
					}
					var translationProp = type.GetPropertyCached("Translations");
					if (translationProp != null)
					{
						Type translationType = translationProp.PropertyType.GetGenericArguments()[0];
						var orderByProp = translationType.GetPropertyCached(parameters.OrderBy.Contains('.') ? parameters.OrderBy.Substring(parameters.OrderBy.LastIndexOf('.') + 1) : parameters.OrderBy);
						if (orderByProp != null)
						{
							Expression<Func<T, string>> expression;
							if (parameters.OrderBy.Contains("."))
							{
								string property = parameters.OrderBy.Substring(0, parameters.OrderBy.LastIndexOf('.'));
								string translationProperty = parameters.OrderBy.Substring(parameters.OrderBy.LastIndexOf('.') + 1);
								expression = items.MakeTranslationExpression(property, translationProperty, parameters.LanguageID);
							}
							else
								expression = items.MakeTranslationExpression(parameters.OrderBy, parameters.LanguageID);
							sorted = parameters.OrderByDirection == Constants.SortDirection.Ascending ? items.OrderBy(expression) : items.OrderByDescending(expression);
						}
					}
				}
			}

			if (sorted == null)
				sorted = items.OrderBy(x => 1);
			return sorted;
		}

		/// <summary>
		/// Makes a dynamic expression for a translation property based on the translation type - DES
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="translationPropertyName"></param>
		/// <param name="languageId"></param>
		/// <returns></returns>
		public static Expression<Func<T, string>> MakeTranslationExpression<T>(this IQueryable<T> source, string translationPropertyName, int languageId)
		{
			return EntitiesExpressionHelper.MakeTranslationExpression<T>(translationPropertyName, languageId);
		}

		public static Expression<Func<T, string>> MakeTranslationExpression<T>(this IQueryable<T> source, string property, string translationPropertyName, int languageId)
		{
			return EntitiesExpressionHelper.MakeTranslationExpression<T>(property, translationPropertyName, languageId);
		}

		public static Expression<Func<T, string>> MakeTermTranslationExpression<T>(this IQueryable<T> source, string termNameProperty, int languageID, ObjectContext context)//, List<TermTranslation> terms)
		{
			return EntitiesExpressionHelper.MakeTermTranslationExpression<T>(termNameProperty, languageID, context);
		}

		public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> items, IPaginatedListParameters parameters)
		{
           
            if (!parameters.OrderBy.IsNullOrEmpty())
            {
                if (parameters.OrderByDirection == Constants.SortDirection.Ascending)
                {
                    if (items.OrderBy(parameters.OrderBy) != null)
                        items = items.OrderBy(parameters.OrderBy);
                }
                else
                {
                    if (items.OrderByDescending(parameters.OrderBy) != null)
                        items = items.OrderByDescending(parameters.OrderBy);
                }
            }

            if (parameters.PageSize.HasValue)
				items = items.Skip(parameters.PageIndex * parameters.PageSize.Value).Take(parameters.PageSize.Value);

            
			return items;
		}

		public static IQueryable<T> ApplyDateRangeFilters<T>(this IQueryable<T> items, string datePropertyName, DateRangeSearchParameters parameters)
		{
			return ApplyDateRangeFilters<T>(items, datePropertyName, datePropertyName, parameters);
		}
		public static IQueryable<T> ApplyDateRangeFilters<T>(this IQueryable<T> items, string startDatePropertyName, string endDatePropertyName, DateRangeSearchParameters parameters)
		{
			if (parameters.StartDate.HasValue)
			{
				DateTime? startDateUTC = parameters.StartDate.LocalToUTC();
				items = items.Where(startDatePropertyName, ComparisonType.GreaterThanOrEqual, parameters.StartDate);
			}
			if (parameters.EndDate.HasValue)
			{
				DateTime? endDateUTC = parameters.EndDate.LocalToUTC();
				items = items.Where(endDatePropertyName, ComparisonType.LessThanOrEqual, parameters.EndDate);
			}

			return items;
		}
	}
}