using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Constraints;
using System.Text.RegularExpressions;
using TestMasterHelpProvider.Graphs;
using WatiN.Core.Extras;

namespace WatiN.Core
{
	public static class WatinIElementContainerExtensions
	{
		#region ElementContainer Extensions

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<UnorderedList>(findBy);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<UnorderedList>(elementId);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<UnorderedList>(elementIdRegex);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this IElementContainer elementContainer, Predicate<UnorderedList> predicate)
		{
			return elementContainer.ElementOfType<UnorderedList>(predicate);
		}

		/// <summary>
		/// Gets a list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static ListItem ListItem(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<ListItem>(findBy);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static ListItem ListItem(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<ListItem>(elementId);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static ListItem ListItem(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<ListItem>(elementIdRegex);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static ListItem ListItem(this IElementContainer elementContainer, Predicate<ListItem> predicate)
		{
			return elementContainer.ElementOfType<ListItem>(predicate);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<OrderedList>(findBy);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<OrderedList>(elementId);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<OrderedList>(elementIdRegex);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this IElementContainer elementContainer, Predicate<OrderedList> predicate)
		{
			return elementContainer.ElementOfType<OrderedList>(predicate);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel1>(findBy);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel1>(elementId);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel1>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this IElementContainer elementContainer, Predicate<HeaderLevel1> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel1>(predicate);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel2>(findBy);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel2>(elementId);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel2>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this IElementContainer elementContainer, Predicate<HeaderLevel2> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel2>(predicate);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel3>(findBy);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel3>(elementId);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel3>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this IElementContainer elementContainer, Predicate<HeaderLevel3> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel3>(predicate);
		}

		/// <summary>
		/// Gets an h4 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel4>(findBy);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel4>(elementId);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel4>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this IElementContainer elementContainer, Predicate<HeaderLevel4> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel4>(predicate);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel5>(findBy);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel5>(elementId);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel5>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this IElementContainer elementContainer, Predicate<HeaderLevel5> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel5>(predicate);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<HeaderLevel6>(findBy);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<HeaderLevel6>(elementId);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<HeaderLevel6>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this IElementContainer elementContainer, Predicate<HeaderLevel6> predicate)
		{
			return elementContainer.ElementOfType<HeaderLevel6>(predicate);
		}

		/// <summary>
		/// Gets an code as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static Code Code(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<Code>(findBy);
		}

		/// <summary>
		/// Gets an code as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static Code Code(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<Code>(elementId);
		}

		/// <summary>
		/// Gets an code as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static Code Code(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<Code>(elementIdRegex);
		}

		/// <summary>
		/// Gets an code as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Code Code(this IElementContainer elementContainer, Predicate<Code> predicate)
		{
			return elementContainer.ElementOfType<Code>(predicate);
		}

		/// <summary>
		/// Gets an pre as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this IElementContainer elementContainer, Constraint findBy)
		{
			return elementContainer.ElementOfType<PreFormattedText>(findBy);
		}

		/// <summary>
		/// Gets an pre as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this IElementContainer elementContainer, string elementId)
		{
			return elementContainer.ElementOfType<PreFormattedText>(elementId);
		}

		/// <summary>
		/// Gets an pre as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this IElementContainer elementContainer, Regex elementIdRegex)
		{
			return elementContainer.ElementOfType<PreFormattedText>(elementIdRegex);
		}

		/// <summary>
		/// Gets an pre as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this IElementContainer elementContainer, Predicate<PreFormattedText> predicate)
		{
			return elementContainer.ElementOfType<PreFormattedText>(predicate);
		}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="findBy"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this IElementContainer elementContainer, Constraint findBy)
        //{
        //    return elementContainer.ElementOfType<ImageMap>(findBy);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="elementId"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this IElementContainer elementContainer, string elementId)
        //{
        //    return elementContainer.ElementOfType<ImageMap>(elementId);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="elementIdRegex"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this IElementContainer elementContainer, Regex elementIdRegex)
        //{
        //    return elementContainer.ElementOfType<ImageMap>(elementIdRegex);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this IElementContainer elementContainer, Predicate<ImageMap> predicate)
        //{
        //    return elementContainer.ElementOfType<ImageMap>(predicate);
        //}

		#endregion
	}
}
