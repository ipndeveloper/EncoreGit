using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WatiN.Core;
using TestMasterHelpProvider;
using System.Text.RegularExpressions;
using WatiN.Core.Extras;

namespace WatiN.Core
{
	public static class WatinElementExtensions
	{
		#region Element Extensions

		/// <summary>
		/// Holds the shift key down, but does not release it until ShiftUp is called.
		/// </summary>
		/// <param name="element"></param>
		public static void ShiftDown(this Element element)
		{
			SendKeys.Send(SendKeysConstants.ShiftDown);
		}

		/// <summary>
		/// Releases the shift key.
		/// </summary>
		/// <param name="element"></param>
		public static void ShiftUp(this Element element)
		{
			SendKeys.Send(SendKeysConstants.ShiftUp);
		}

		/// <summary>
		/// Shift+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void ShiftClick(this Element element)
		{
			SendKeys.Send(SendKeysConstants.ShiftDown);

			element.Click();

			SendKeys.Send(SendKeysConstants.ShiftUp);
		}

		/// <summary>
		/// Holds the alt key down, but does not release it until AltUp is called.
		/// </summary>
		/// <param name="element"></param>
		public static void AltDown(this Element element)
		{
			SendKeys.Send(SendKeysConstants.AltDown);
		}

		/// <summary>
		/// Releases the alt key.
		/// </summary>
		/// <param name="element"></param>
		public static void AltUp(this Element element)
		{
			SendKeys.Send(SendKeysConstants.AltUp);
		}

		/// <summary>
		/// Alt+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void AltClick(this Element element)
		{
			SendKeys.Send(SendKeysConstants.AltDown);

			element.Click();

			SendKeys.Send(SendKeysConstants.AltUp);
		}

		/// <summary>
		/// Holds the ctrl key down, but does not release it until CtrlUp is called.
		/// </summary>
		/// <param name="element"></param>
		public static void CtrlDown(this Element element)
		{
			SendKeys.Send(SendKeysConstants.CtrlDown);
		}

		/// <summary>
		/// Releases the ctrl key.
		/// </summary>
		/// <param name="element"></param>
		public static void CtrlUp(this Element element)
		{
			SendKeys.Send(SendKeysConstants.CtrlUp);
		}

		/// <summary>
		/// Ctrl+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void CtrlClick(this Element element)
		{
			SendKeys.Send(SendKeysConstants.CtrlDown);

			element.Click();

			SendKeys.Send(SendKeysConstants.CtrlUp);
		}

		/// <summary>
		/// Shift+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void ShiftClickNoWait(this Element element)
		{
			SendKeys.Send(SendKeysConstants.ShiftDown);

			element.ClickNoWait();

			SendKeys.Send(SendKeysConstants.ShiftUp);
		}

		/// <summary>
		/// Alt+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void AltClickNoWait(this Element element)
		{
			SendKeys.Send(SendKeysConstants.AltDown);

			element.ClickNoWait();

			SendKeys.Send(SendKeysConstants.AltUp);
		}

		/// <summary>
		/// Ctrl+Clicks on the element.
		/// </summary>
		/// <param name="element"></param>
		public static void CtrlClickNoWait(this Element element)
		{
			SendKeys.Send(SendKeysConstants.CtrlDown);

			element.ClickNoWait();

			SendKeys.Send(SendKeysConstants.CtrlUp);
		}

		#endregion

		#region Extensions for individual elements

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static string GetName(this Element element)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="area"></param>
		/// <returns></returns>
		public static string GetName(this Area area)
		{
			return WatinFrameworkConstants.AreaTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		public static string GetName(this Body body)
		{
			return WatinFrameworkConstants.BodyTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static string GetName(this Button button)
		{
			return WatinFrameworkConstants.ButtonTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="checkBox"></param>
		/// <returns></returns>
		public static string GetName(this CheckBox checkBox)
		{
			return WatinFrameworkConstants.CheckBoxTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static string GetName(this Code code)
		{
			return WatinFrameworkConstants.CodeTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="div"></param>
		/// <returns></returns>
		public static string GetName(this Div div)
		{
			return WatinFrameworkConstants.DivTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="fileUpload"></param>
		/// <returns></returns>
		public static string GetName(this FileUpload fileUpload)
		{
			return WatinFrameworkConstants.FileUploadTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public static string GetName(this Form form)
		{
			return WatinFrameworkConstants.FormTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="frame"></param>
		/// <returns></returns>
		public static string GetName(this Frame frame)
		{
			return WatinFrameworkConstants.FrameTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel1 header)
		{
			return WatinFrameworkConstants.HeaderLevel1TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel2 header)
		{
			return WatinFrameworkConstants.HeaderLevel2TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel3 header)
		{
			return WatinFrameworkConstants.HeaderLevel3TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel4 header)
		{
			return WatinFrameworkConstants.HeaderLevel4TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel5 header)
		{
			return WatinFrameworkConstants.HeaderLevel5TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetName(this HeaderLevel6 header)
		{
			return WatinFrameworkConstants.HeaderLevel6TagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static string GetName(this Image image)
		{
			return WatinFrameworkConstants.ImageTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static string GetName(this Label label)
		{
			return WatinFrameworkConstants.LabelTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="link"></param>
		/// <returns></returns>
		public static string GetName(this Link link)
		{
			return WatinFrameworkConstants.LinkTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="listItem"></param>
		/// <returns></returns>
		public static string GetName(this ListItem listItem)
		{
			return WatinFrameworkConstants.ListItemTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public static string GetName(this Option option)
		{
			return WatinFrameworkConstants.OptionTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="orderedList"></param>
		/// <returns></returns>
		public static string GetName(this OrderedList orderedList)
		{
			return WatinFrameworkConstants.OrderedListTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="para"></param>
		/// <returns></returns>
		public static string GetName(this Para para)
		{
			return WatinFrameworkConstants.ParaTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="pre"></param>
		/// <returns></returns>
		public static string GetName(this PreFormattedText pre)
		{
			return WatinFrameworkConstants.PreformattedTextTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="radioButton"></param>
		/// <returns></returns>
		public static string GetName(this RadioButton radioButton)
		{
			return WatinFrameworkConstants.RadioButtonTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="selectList"></param>
		/// <returns></returns>
		public static string GetName(this SelectList selectList)
		{
			return WatinFrameworkConstants.SelectListTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="span"></param>
		/// <returns></returns>
		public static string GetName(this Span span)
		{
			return WatinFrameworkConstants.SpanTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static string GetName(this Table table)
		{
			return WatinFrameworkConstants.TableTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="tableBody"></param>
		/// <returns></returns>
		public static string GetName(this TableBody tableBody)
		{
			return WatinFrameworkConstants.TableBodyTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="tableCell"></param>
		/// <returns></returns>
		public static string GetName(this TableCell tableCell)
		{
			return WatinFrameworkConstants.TableCellTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="tableRow"></param>
		/// <returns></returns>
		public static string GetName(this TableRow tableRow)
		{
			return WatinFrameworkConstants.TableRowTagName;
		}

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <param name="textField"></param>
		/// <returns></returns>
		public static string GetName(this TextField textField)
		{
			return WatinFrameworkConstants.TextFieldTagName;
		}

		/// <summary>
		/// Selects an option and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		public static void Select(this Option element)
		{
			element.Select();

			InvokeEventViaJquery((element.Parent as SelectList), "change");
		}

		/// <summary>
		/// Selects an option and doesn't wait and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		public static void SelectNoWait(this Option element)
		{
			element.SelectNoWait();

			InvokeEventViaJquery((element.Parent as SelectList), "change");
		}

		/// <summary>
		/// Selects an item from the given select list and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void Select(this SelectList element, string value)
		{
			element.Select(value);

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Selects an item from the given select list and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="regex"></param>
		public static void Select(this SelectList element, Regex regex)
		{
			element.Select(regex);

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Selects an item from the given select list by its value and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SelectByValue(this SelectList element, string value)
		{
			element.SelectByValue(value);

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Selects an item from the given select list by its value and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="regex"></param>
		public static void SelectByValue(this SelectList element, Regex regex)
		{
			element.SelectByValue(regex);

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Checks a checkbox and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		public static void Check(this CheckBox element)
		{
			element.Checked = true;

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Unchecks a checkbox and invokes the Change event.
		/// </summary>
		/// <param name="element"></param>
		public static void Uncheck(this CheckBox element)
		{
			element.Checked = false;

			InvokeEventViaJquery(element, "change");
		}

		/// <summary>
		/// Types text in a text field, invoking keydown, keypress, keyup events.
		/// </summary>
		/// <param name="element"></param>
		public static void TypeText(this TextField element, string text)
		{
			string elementId = element.Id;

			if (String.IsNullOrEmpty(elementId))
			{
				elementId = String.Format("{0}{1}", element.TagName, element.GetHashCode());

				element.SetAttributeValue("id", elementId);
			}

			char[] chars = text.ToCharArray();
			byte[] bytes = ASCIIEncoding.ASCII.GetBytes(chars);

			element.SetAttributeValue("value", String.Format("{0}", chars[0]));

			for (int index = 1; index < bytes.Length; index++)
			{
				char nextChar = chars[index];
				byte nextByte = bytes[index];
				string elementValue = String.Concat(element.GetAttributeValue("value"), String.Format("{0}", nextChar));

				element.SetAttributeValue("value", elementValue);

				string eventScriptKeyDown = String.Format("$(\"#{0}\").trigger($.Event({{ type: \"keydown\", which: {1} }}));", elementId, nextByte);
				string eventScriptKeyPress = String.Format("$(\"#{0}\").trigger($.Event({{ type: \"keypress\", which: {1} }}));", elementId, nextByte);
				string eventScriptKeyUp = String.Format("$(\"#{0}\").trigger($.Event({{ type: \"keyup\", which: {1} }}));", elementId, nextByte);

				WatiNContext.GetContext().Browser.RunScript(eventScriptKeyDown);

				Utils.Wait(75);

				WatiNContext.GetContext().Browser.RunScript(eventScriptKeyPress);

				Utils.Wait(75);

				WatiNContext.GetContext().Browser.RunScript(eventScriptKeyUp);

				Utils.Wait(75);
			}
		}

		/// <summary>
		/// Focuses on a focusable element.
		/// </summary>
		/// <param name="element"></param>
		public static void Focus(this Element element)
		{
			InvokeEventViaJquery(element, "focus");
		}

		/// <summary>
		/// Clicks on a link by invoking its Click event.
		/// </summary>
		/// <param name="element"></param>
		public static void Click(this Link element)
		{
			InvokeEventViaJquery(element, "click");
		}

		/// <summary>
		/// Invokes an event.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="eventName"></param>
		public static void InvokeEventViaJquery(Element element, string eventName)
		{
			string elementId = element.Id;

			if (String.IsNullOrEmpty(elementId))
			{
				elementId = String.Format("{0}{1}", element.TagName, element.GetHashCode());

				element.SetAttributeValue("id", elementId);
			}

			string script = String.Format("$(\"#{0}\").{1}();", elementId, eventName);

			WatiNContext.GetContext().Browser.RunScript(script);

			Utils.Wait(1500);
		}

		#endregion
	}
}
