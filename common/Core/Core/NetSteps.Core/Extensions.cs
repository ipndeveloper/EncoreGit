using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Various extension methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets the fully qualified, human readable name for a delegate.
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static string GetFullName(this Delegate d)
		{
			Contract.Requires<ArgumentNullException>(d != null);
			Contract.Requires<ArgumentException>(d.Method != null);
			Contract.Requires<ArgumentException>(d.Target != null);
			
			return String.Concat(d.Target.GetType().FullName, ".", d.Method.Name, "()");
		}

		/// <summary>
		/// Removes a string from the end of another string if present.
		/// </summary>
		/// <param name="target">The target string.</param>
		/// <param name="value">The value to remove.</param>
		/// <returns>the target string with the value removed</returns>
		public static string RemoveTrailing(this string target, string value)
		{
			if (!String.IsNullOrEmpty(target) && !String.IsNullOrEmpty(value)
				&& target.EndsWith(value))
			{
				return target.Substring(0, target.Length - value.Length);
			}
			return target;
		}

		/// <summary>
		/// Determines if the arrays are equal or if the items in two different arrays
		/// are equal.
		/// </summary>
		/// <typeparam name="T">Item type T</typeparam>
		/// <param name="lhs">Left-hand comparand</param>
		/// <param name="rhs">Right-hand comparand</param>
		/// <returns><b>true</b> if the arrays are equal or if the items in the arrays are equal.</returns>
		public static bool EqualsOrItemsEqual<T>(this T[] lhs, T[] rhs)
		{
			bool result = Object.Equals(lhs, rhs);
			if (result == false && lhs != null && rhs != null
				&& lhs.LongLength == rhs.LongLength)
			{
				if (lhs.Length == 0)
				{ // two empty arrays are equal.
					result = true;
				}
				else
				{
					var comparer = EqualityComparer<T>.Default;
					for (int i = 0; i < lhs.LongLength; i++)
					{
						result = comparer.Equals(lhs[i], rhs[i]);
						if (!result) break;
					}
				}
			}
			return result;
		}
		
		/// <summary>
		/// Produces a combined hashcode from the enumerated items.
		/// </summary>
		/// <typeparam name="T">element type T</typeparam>
		/// <param name="items">an enumerable</param>
		/// <param name="seed">the hash seed (starting value)</param>
		/// <returns>the combined hashcode</returns>
		public static int CalculateCombinedHashcode<T>(this IEnumerable<T> items, int seed)
		{
			if (items == null) return seed;
			
			var comp = EqualityComparer<T>.Default;

			int prime = Constants.RandomPrime;
			int result = seed ^ (items.GetHashCode() * prime);
			foreach (var item in items)
			{
				if (!comp.Equals(default(T), item))
				{
					result ^= item.GetHashCode() * prime;
				}
			}
			return result;
		}
				
		/// <summary>
		/// Counts the number of bits turned on.
		/// </summary>
		/// <param name="value">a value</param>
		/// <returns>number of bits turned on</returns>
		[CLSCompliant(false)]
		public static int CountBitsInFlag(this uint value)
		{
			uint c = 0;
			value = value - ((value >> 1) & 0x55555555);                // reuse input as temporary
			value = (value & 0x33333333) + ((value >> 2) & 0x33333333); // temp
			c = ((value + (value >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
			unchecked { return (int)c; }
		}

		/// <summary>
		/// Counts the number of bits turned on.
		/// </summary>
		/// <param name="value">a value</param>
		/// <returns>number of bits turned on</returns>
		public static int CountBitsInFlag(this int value)
		{
			uint v = (uint)value;
			uint c = 0;
			v = v - ((v >> 1) & 0x55555555);
			v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
			c = ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
			unchecked { return (int)c; }
		}

		/// <summary>
		/// Gets a member from the expression given.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="expression">the expression</param>
		/// <returns>the expression's target member</returns>
		[SuppressMessage("Microsoft.Design", "CA1011")]
		[SuppressMessage("Microsoft.Design", "CA1006")]
		public static MemberInfo GetMemberFromExpression<T>(this Expression<Func<T, object>> expression)
		{
			Contract.Requires<ArgumentNullException>(expression != null);

			if (expression.Body is MemberExpression)
			{
				MemberExpression memberExpression = (MemberExpression)expression.Body;
				return memberExpression.Member;
			}
			else
			{
				UnaryExpression unaryExpression = (UnaryExpression)expression.Body;

				if (unaryExpression.Operand is MemberExpression)
				{
					MemberExpression memberExpression = (MemberExpression)unaryExpression.Operand;
					return memberExpression.Member;
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a dynamic object over the given JSON.
		/// </summary>
		/// <param name="json">JSON input</param>
		/// <returns>a dynamic object</returns>
		public static dynamic JsonToDynamic(this string json)
		{
			object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			if (obj is string)
			{
				return obj as string;
			}
			else
			{
				return ConvertJson((JToken)obj);
			}
		}

		static dynamic ConvertJson(JToken token)
		{
			if (token is JValue)
			{
				return ((JValue)token).Value;
			}
			else if (token is JObject)
			{
				var expando = new ExpandoObject();
				(from childToken in ((JToken)token) where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
				{
					((IDictionary<string, object>)expando).Add(property.Name, ConvertJson(property.Value));
				});
				return expando;
			}
			else if (token is JArray)
			{
				var items = new List<ExpandoObject>();
				foreach (JToken arrayItem in ((JArray)token))
				{
					dynamic value = ConvertJson(arrayItem);
					if (arrayItem is JValue)
					{
						var wrapper = new ExpandoObject();
						((IDictionary<string, object>)wrapper).Add("Value", value);
						value = wrapper;
					}
					items.Add(value);
				}
				return items;
			}
			throw new ArgumentException(string.Format("Unknown token type '{0}'",
				token.GetType()), "token"
				);
		}

		/// <summary>
		/// Converts the source object to JSON
		/// </summary>
		/// <param name="source">the source</param>
		/// <returns>the JSON representation of the source</returns>
		public static string ToJson(this object source)
		{
			return JsonConvert.SerializeObject(source, Formatting.Indented, new IsoDateTimeConverter());
		}

		/// <summary>
		/// Double quotes the given string, delimiting inner quotes.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string DoubleQuote(this string source)
		{
			return (String.IsNullOrEmpty(source))
				? "\"\""
				: String.Concat("\"", source.Replace("\"", "\\\""), "\"");
		}

		/// <summary>
		/// Converts an enumerable to a readonly collection.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="collection">the collection</param>
		/// <returns>returns a read-only collection</returns>
		public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> collection)
		{
			ReadOnlyCollection<T> roc = collection as ReadOnlyCollection<T>;
			if (roc == null)
			{
				if (collection == null)
				{
					roc = new List<T>(Enumerable.Empty<T>()).AsReadOnly();
				}
				else if (collection is List<T>)
				{
					roc = (collection as List<T>).AsReadOnly();
				}
				else
				{
					roc = new List<T>(collection).AsReadOnly();
				}
			}
			return roc;
		}
	}

}
