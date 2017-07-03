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
	public static class WatinDocumentExtensions
	{
		#region Document Extensions

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this Document document, Constraint findBy)
		{
			return document.ElementOfType<UnorderedList>(findBy);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this Document document, string elementId)
		{
			return document.ElementOfType<UnorderedList>(elementId);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<UnorderedList>(elementIdRegex);
		}

		/// <summary>
		/// Gets an unordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static UnorderedList UnorderedList(this Document document, Predicate<UnorderedList> predicate)
		{
			return document.ElementOfType<UnorderedList>(predicate);
		}

		/// <summary>
		/// Gets a list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static ListItem ListItem(this Document document, Constraint findBy)
		{
			return document.ElementOfType<ListItem>(findBy);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static ListItem ListItem(this Document document, string elementId)
		{
			return document.ElementOfType<ListItem>(elementId);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static ListItem ListItem(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<ListItem>(elementIdRegex);
		}

		/// <summary>
		/// Gets an list item as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static ListItem ListItem(this Document document, Predicate<ListItem> predicate)
		{
			return document.ElementOfType<ListItem>(predicate);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this Document document, Constraint findBy)
		{
			return document.ElementOfType<OrderedList>(findBy);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this Document document, string elementId)
		{
			return document.ElementOfType<OrderedList>(elementId);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<OrderedList>(elementIdRegex);
		}

		/// <summary>
		/// Gets an ordered list as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static OrderedList OrderedList(this Document document, Predicate<OrderedList> predicate)
		{
			return document.ElementOfType<OrderedList>(predicate);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel1>(findBy);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel1>(elementId);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel1>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h1 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel1 HeaderLevel1(this Document document, Predicate<HeaderLevel1> predicate)
		{
			return document.ElementOfType<HeaderLevel1>(predicate);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel2>(findBy);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel2>(elementId);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel2>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h2 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel2 HeaderLevel2(this Document document, Predicate<HeaderLevel2> predicate)
		{
			return document.ElementOfType<HeaderLevel2>(predicate);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel3>(findBy);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel3>(elementId);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel3>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel3 HeaderLevel3(this Document document, Predicate<HeaderLevel3> predicate)
		{
			return document.ElementOfType<HeaderLevel3>(predicate);
		}

		/// <summary>
		/// Gets an h4 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel4>(findBy);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel4>(elementId);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel4>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h3 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel4 HeaderLevel4(this Document document, Predicate<HeaderLevel4> predicate)
		{
			return document.ElementOfType<HeaderLevel4>(predicate);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel5>(findBy);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel5>(elementId);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel5>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h5 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel5 HeaderLevel5(this Document document, Predicate<HeaderLevel5> predicate)
		{
			return document.ElementOfType<HeaderLevel5>(predicate);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this Document document, Constraint findBy)
		{
			return document.ElementOfType<HeaderLevel6>(findBy);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this Document document, string elementId)
		{
			return document.ElementOfType<HeaderLevel6>(elementId);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<HeaderLevel6>(elementIdRegex);
		}

		/// <summary>
		/// Gets an h6 as an Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static HeaderLevel6 HeaderLevel6(this Document document, Predicate<HeaderLevel6> predicate)
		{
			return document.ElementOfType<HeaderLevel6>(predicate);
		}

		/// <summary>
		/// Gets a code Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static Code Code(this Document document, Constraint findBy)
		{
			return document.ElementOfType<Code>(findBy);
		}

		/// <summary>
		/// Gets a Code Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static Code Code(this Document document, string elementId)
		{
			return document.ElementOfType<Code>(elementId);
		}

		/// <summary>
		/// Gets a code Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static Code Code(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<Code>(elementIdRegex);
		}

		/// <summary>
		/// Gets a code Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Code Code(this Document document, Predicate<Code> predicate)
		{
			return document.ElementOfType<Code>(predicate);
		}

		/// <summary>
		/// Gets a pre Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this Document document, Constraint findBy)
		{
			return document.ElementOfType<PreFormattedText>(findBy);
		}

		/// <summary>
		/// Gets a pre Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this Document document, string elementId)
		{
			return document.ElementOfType<PreFormattedText>(elementId);
		}

		/// <summary>
		/// Gets a pre Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this Document document, Regex elementIdRegex)
		{
			return document.ElementOfType<PreFormattedText>(elementIdRegex);
		}

		/// <summary>
		/// Gets a pre Element.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static PreFormattedText PreFormattedText(this Document document, Predicate<PreFormattedText> predicate)
		{
			return document.ElementOfType<PreFormattedText>(predicate);
		}

		/// <summary>
		/// Gets a CKEditor control.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="findBy"></param>
		/// <returns></returns>
		public static CkEditor CKEditorControl(this Document document, Constraint findBy)
		{
			return new CkEditor(document.Frame(findBy));
		}

		/// <summary>
		/// Gets a CKEditor control.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public static CkEditor CKEditorControl(this Document document, string elementId)
		{
			return new CkEditor(document.Frame(elementId));
		}

		/// <summary>
		/// Gets a CKEditor control.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="elementIdRegex"></param>
		/// <returns></returns>
		public static CkEditor CKEditorControl(this Document document, Regex elementIdRegex)
		{
			return new CkEditor(document.Frame(elementIdRegex));
		}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="findBy"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this Document document, Constraint findBy)
        //{
        //    return document.ElementOfType<ImageMap>(findBy);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="elementId"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this Document document, string elementId)
        //{
        //    return document.ElementOfType<ImageMap>(elementId);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="elementIdRegex"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this Document document, Regex elementIdRegex)
        //{
        //    return document.ElementOfType<ImageMap>(elementIdRegex);
        //}

        ///// <summary>
        ///// Gets an image map as an Element.
        ///// </summary>
        ///// <param name="document"></param>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public static ImageMap ImageMap(this Document document, Predicate<ImageMap> predicate)
        //{
        //    return document.ElementOfType<ImageMap>(predicate);
        //}

		#endregion
	}
}
