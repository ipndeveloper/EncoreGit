using System;
using System.Collections.Concurrent;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Expressions
{
	public class EntitiesExpressionHelper
	{
		public static Expression<Func<T, string>> MakeTranslationExpression<T>(string translationPropertyName, int languageID)
		{
			return MakeTranslationExpression<T>(null, translationPropertyName, languageID);
		}

        private static readonly ConcurrentDictionary<Tuple<Type, string>, Expression> translationExpressionCache = new ConcurrentDictionary<Tuple<Type, string>, Expression>();
		public static Expression<Func<T, string>> MakeTranslationExpression<T>(string property, string translationPropertyName, int languageID)
		{
			var tType = typeof(T);
			var keyLookup = new Tuple<Type, string>(tType, property);

		    Expression expr;
		    translationExpressionCache.TryGetValue(keyLookup, out expr);
			if (expr != null)
				return expr as Expression<Func<T, string>>;

		    Func<Tuple<Type, string>, Expression> func = key =>
		        {
                    //x properties
			        var x = Expression.Parameter(tType, "x");
			        Expression xProp = x;
			        Type xPropType = tType;
			        if (!string.IsNullOrEmpty(property))
			        {
				        string[] props = property.Split('.');
				        foreach (string prop in props)
				        {
					        var p = xPropType.GetPropertyCached(prop);
					        if (p == null)
						        throw new ArgumentException(string.Format("Property '{0}' does not exist on type '{1}'", prop, xPropType.Name));
					        xProp = Expression.Property(xProp, prop);
					        xPropType = p.PropertyType;
				        }
			        }
			        var xTranslations = Expression.Property(xProp, "Translations");

			        //translation properties
			        var translationType = xPropType.GetPropertyCached("Translations").PropertyType.GetGenericArguments()[0];
			        var t = Expression.Parameter(translationType, "t");
			        var tLanguageID = Expression.Property(t, "LanguageID");

			        //methods
			        var countMethod = typeof(Enumerable).GetMethodsCached().First(m => m.Name == "Count" && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 1).MakeGenericMethodCached(translationType);
			        var countMethodWithLambda = typeof(Enumerable).GetMethodsCached().First(m => m.Name == "Count" && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2).MakeGenericMethodCached(translationType);
			        var firstMethod = typeof(Enumerable).GetMethodsCached().First(m => m.Name == "FirstOrDefault" && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 1).MakeGenericMethodCached(translationType);
			        var firstMethodWithLambda = typeof(Enumerable).GetMethodsCached().First(m => m.Name == "FirstOrDefault" && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2).MakeGenericMethodCached(translationType);

			        var tLanguageIDEqualsLanguageID = Expression.Equal(tLanguageID, Expression.Constant(languageID));
			        var tLanguageIDEqualsLanguageIDLambda = Expression.Lambda(tLanguageIDEqualsLanguageID, t);

			        //x.Translations.Count(t => t.LanguageID == languageID) == 0
			        var count = Expression.Call(countMethodWithLambda, xTranslations, tLanguageIDEqualsLanguageIDLambda);
			        var countEqualsZero = Expression.Equal(count, Expression.Constant(0));

			        /************************** INNER CONDITIONAL ***********************/
			        // ? x.Translations.Count > 0
			        var countGreaterThanZero = Expression.GreaterThan(Expression.Call(countMethod, xTranslations), Expression.Constant(0));

			        // ? x.Translations.First().TranslationPropertyName
			        var firstTranslationTranslationProperty = Expression.Property(Expression.Call(firstMethod, xTranslations), translationPropertyName);

			        // : ""
			        var emptyString = Expression.Constant(string.Empty);

			        var innerConditional = Expression.Condition(countGreaterThanZero, firstTranslationTranslationProperty, emptyString);
			        /************************ END INNER CONDITIONAL *********************/

			        // : x.Translations.First(t => t.LanguageID == languageID).TranslationPropertyName
			        var firstMatchingTranslationTranslationProperty = Expression.Property(Expression.Call(firstMethodWithLambda, xTranslations, tLanguageIDEqualsLanguageIDLambda), translationPropertyName);


			        var outerConditional = Expression.Condition(countEqualsZero, innerConditional, firstMatchingTranslationTranslationProperty);
			        var body = Expression.Lambda<Func<T, string>>(outerConditional, x);

			        return body;
		        };

            return translationExpressionCache.AddOrUpdate(keyLookup, func, (key, oldValue) => func(key)) as Expression<Func<T, string>> ;
		}

		public static Expression<Func<T, string>> MakeTermTranslationExpression<T>(string termNameProperty, int languageID, ObjectContext context)
		{
			string[] props = termNameProperty.Split('.');
			var x = Expression.Parameter(typeof(T), "x");
			Expression termNameProp = x;
			foreach (string prop in props)
			{
				termNameProp = Expression.Property(termNameProp, prop);
			}

			var translations = Expression.Property(Expression.Constant(context), "TermTranslations");
			var translation = Expression.Parameter(typeof(TermTranslation), "t");
			var translationTermName = Expression.Property(translation, "TermName");
			var translationLanguageID = Expression.Property(translation, "LanguageID");

			var translationTermNameEquals = Expression.Equal(translationTermName, termNameProp);

			var getTranslation = Expression.AndAlso(translationTermNameEquals, Expression.Equal(translationLanguageID, Expression.Constant(languageID)));

			var firstOrDefault = typeof(Enumerable).GetMethodsCached().First(m => m.Name == "FirstOrDefault" && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2);

			var firstOrDefaultExpression = Expression.Call(firstOrDefault.MakeGenericMethod(typeof(TermTranslation)), translations, Expression.Lambda(getTranslation, translation));

			return Expression.Lambda<Func<T, string>>(Expression.Property(firstOrDefaultExpression, "Term"), x);
		}
	}
}
