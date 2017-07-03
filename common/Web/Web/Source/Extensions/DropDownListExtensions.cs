using System;
using System.Web.UI.WebControls;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: DropDownList Extensions
	/// Created: 05-18-2009
	/// </summary>
	public static class DropDownListExtensions
	{
		#region Validation Methods
		public static bool IsEmpty(this DropDownList dropDownList)
		{
			return (dropDownList.SelectedValue.Trim() == string.Empty);
		}
		#endregion

		#region Conversion Methods
		public static int ToInt(this DropDownList dropDownList)
		{
			if (dropDownList.SelectedValue.IsValidInt())
				return Convert.ToInt32(dropDownList.SelectedValue);
			else
				return 0;
		}

		public static string ToCleanString(this DropDownList dropDownList)
		{
			return dropDownList.SelectedValue.Trim();
		}

		public static char ToChar(this DropDownList dropDownList)
		{
			try
			{
				return Convert.ToChar(dropDownList.SelectedValue);
			}
			catch
			{
				return Convert.ToChar('\0');
			}
		}
		#endregion

		public static bool SetSelectedValue(this DropDownList dropDownList, int value)
		{
			return SetSelectedValue(dropDownList, value.ToString());
		}
		public static bool SetSelectedValue(this DropDownList dropDownList, string value)
		{
			try
			{
				dropDownList.SelectedIndex = dropDownList.Items.IndexOf(dropDownList.Items.FindByValue(value));
				return true;
			}
			catch
			{
				try
				{
					dropDownList.SelectedIndex = 1;
				}
				catch { }
				return false;
			}
		}

		public static void AddItem(this DropDownList dropDownList, string text, string value)
		{
			dropDownList.Items.Add(new ListItem(text, value));
		}

		public static void BindData(this DropDownList dropDownList, string dataTextField, string dataValueField, object dataSource)
		{
			dropDownList.DataTextField = dataTextField;
			dropDownList.DataValueField = dataValueField;
			dropDownList.DataSource = dataSource;
			dropDownList.DataBind();
		}

		#region Fill Methods
		/// <summary>
		/// For populating dropdowns with all the months. - JHE
		/// </summary>
		/// <param name="dropDownList"></param>
		/// 

		public static void FillMonths(this DropDownList dropDownList, NetSteps.Common.Constants.MonthDisplayType monthType)
		{
			ListItem li;
			//max:  I added the ability for the months to appear in the dropdown with their 3 letter acronym names instead of the numbers:
			for (int i = 1; i <= 12; i++)
			{
				li = new ListItem(GetMonthName(i, monthType), i.ToString());
				dropDownList.Items.Add(li);
			}
		}

		public static string GetMonthName(int monthID, NetSteps.Common.Constants.MonthDisplayType monthType)
		{
			try
			{
				if (monthType == NetSteps.Common.Constants.MonthDisplayType.NumberOnly)
					return monthID.ToString();
				else
				{
					DateTime dtMonth = new DateTime(DateTime.Now.Year, monthID, 1);
					string monthName = dtMonth.ToString("MMMM");
					if (monthType == NetSteps.Common.Constants.MonthDisplayType.FullName)
						return monthName;
					else
						return monthName.ToUpper().Substring(0, 3);
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// For populating dropdowns with all the days of a month. - JHE
		/// </summary>
		/// <param name="dropDownList"></param>
		public static void FillDays(this DropDownList dropDownList)
		{
			ListItem li;
			for (int i = 1; i <= 31; i++)
			{
				li = new ListItem(i.ToString(), i.ToString());
				dropDownList.Items.Add(li);
			}
		}

		public static void FillDays(this DropDownList dropDownList, int fromDay, int toDay, bool addSuffix)
		{
			ListItem li;
			for (int i = fromDay; i <= toDay; i++)
			{
				li = new ListItem((addSuffix) ? i.ToString() + i.Suffix() : i.ToString(), i.ToString());
				dropDownList.Items.Add(li);
			}
		}

		/// <summary>
		/// For populating dropdowns with a selection of years. - JHE
		/// </summary>
		/// <param name="dropDownList"></param>
		public static void FillYears(this DropDownList dropDownList, int yearsBack, int yearsForward, bool use4DigitYear)
		{
			int yearNum = DateTime.Now.Year;
			string year = string.Empty;
			ListItem li;
			for (int i = yearNum; i >= yearNum - yearsBack; i--)
			{
				year = i.ToString();
				if (!use4DigitYear)
					year = year.Substring(2, 2);
				li = new ListItem(i.ToString(), year);
				dropDownList.Items.Add(li);
			}
		}

		#endregion

		/// <summary>
		/// Removes duplicate items from DropDownList. - JHE
		/// Taken from: http://dotnetguts.blogspot.com/2006/10/removing-duplicates-item-from.html
		/// </summary>
		/// <param name="dropDownList"></param>
		public static void RemoveDuplicateItems(this DropDownList dropDownList)
		{
			for (int i = 0; i < dropDownList.Items.Count; i++)
			{
				dropDownList.SelectedIndex = i;
				string str = dropDownList.SelectedItem.ToString();
				for (int counter = i + 1; counter < dropDownList.Items.Count; counter++)
				{
					dropDownList.SelectedIndex = counter;
					string compareStr = dropDownList.SelectedItem.ToString();
					if (str == compareStr)
					{
						dropDownList.Items.RemoveAt(counter);
						counter = counter - 1;
					}
				}
			}
		}

		/// <summary>
		/// Sort items in DropDownList by ListItem.Value - JHE
		/// </summary>
		/// <param name="dropDownList"></param>
		public static void SortByItemText(this DropDownList dropDownList)
		{
			ListItem[] items = new ListItem[dropDownList.Items.Count];
			dropDownList.Items.CopyTo(items, 0);
			dropDownList.Items.Clear();
			Array.Sort(items, (x, y) => { return x.Text.CompareTo(y.Text); });
			dropDownList.Items.AddRange(items);
		}

		/// <summary>
		/// Sort items in DropDownList by ListItem.Value and removes duplicate items. - JHE
		/// </summary>
		/// <param name="dropDownList"></param>
		public static void SortAndRemoveDuplicates(this DropDownList dropDownList)
		{
			dropDownList.SortByItemText();
			dropDownList.RemoveDuplicateItems();
		}
	}
}
