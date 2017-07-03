using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using WatiN.Core;
using System.Net;
using System.Net.Cache;

namespace NetSteps.Testing.Integration
{    
    /// <summary>
    /// Class related to all types of custom control operations.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Delegate methods for Page retry operation.
        /// </summary>
        public delegate void ExecuteSteps();
        public delegate bool IsConditionMet();

        #region String

        //public static string RemoveSpaces(this string originalString)
        //{
        //    return originalString.Replace(" ", "");
        //}

        #endregion String

        #region Browser

        public static TPage Navigate<TPage>(this Browser browser, string url, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            browser.GoTo(url);
            Thread.Sleep(2000);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public static TPage CustomRefresh<TPage>(this Browser browser) where TPage: NS_Page, new()
        {
            browser.Refresh();
            return Util.GetPage<TPage>();
        }

        public static int? CustomWaitForComplete(this Browser browser, int? timeout = null)
        {
            int wait = 0;
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            if (timeout > 0)
            {
                var start = DateTime.Now;
                browser.WaitForComplete((int)timeout);
                wait = (DateTime.Now - start).Seconds;
                timeout -= wait;
                if (timeout <= 0)
                    throw new TimeoutException(timeout.ToString());
                if (Util.LogLevel() > 0)
                {
                    Util.LogDoneMessage(string.Format("CustomWaitForComplete {0}, seconds", wait));
                }
            }
            return timeout;
        }


        #endregion Browser

        #region Link

        public static bool HasValidHref(this Link link, HttpStatusCode responseCode = HttpStatusCode.OK)
        {
            link.CustomWaitForVisibility();
            return Util.ValidateUrl(link.Url, responseCode);
        }

        #endregion Link

        #region Image

        public static bool HasValidSource(this Image img, HttpStatusCode responseCode = HttpStatusCode.OK)
        {
            img.CustomWaitForVisibility();
            return Util.ValidateUrl(img.Src);
        }

        #endregion

        #region ImageCollection

        public static bool IsVisible(this ImageCollection elements)
        {
            bool visible = false;
            foreach (Element element in elements)
            {
                if (element.IsVisible())
                {
                    visible = true;
                    break;
                }
            }
            return visible;
        }

        #endregion ImageCollection

        #region TextField

        public static int? CustomSetTextHelper(this TextField element, string txtValue, int? timeout = null, bool visible = true)
        {
            if (visible)
            {
                timeout = element.CustomWaitForVisibility(visible, timeout);
            }
            else
            {
                timeout = element.CustomWaitForExist();
            }
            element.TypeText(txtValue);
            Util.LogDoneMessage(String.Format("Enter'{0}' in text field '{1}'.", txtValue, element.Name));
            return timeout;
        }

        public static int? CustomSetTextQuicklyHelper(this TextField element, string txtValue, int? timeout = null, bool visible = true)
        {
            if (visible)
            {
                timeout = element.CustomWaitForVisibility(visible, timeout);
            }
            else
            {
                timeout = element.CustomWaitForExist();
            }
            element.Value = txtValue;
            Util.LogDoneMessage(String.Format("Enter '{0}' in text field '{1}' ", txtValue, element.Name));
            return timeout;
        }

        public static void CustomSetPasswordTextHelper(this TextField element, string txtValue)
        {
            element.CustomWaitForExist();
            element.Value = txtValue;
            Util.LogDoneMessage(String.Format("Enter '{0}' in text field '{1}'.", txtValue, "password"));

        }

        #endregion TextField

        #region RadioButton

        public static int? CustomSelectRadioButton(this RadioButton element, bool value = true, int? timeout = null)
        {
            timeout = element.CustomWaitForVisibility(true, timeout);
            element.Checked = value;
            timeout = Util.Browser.CustomWaitForComplete(timeout);
            Util.LogDoneMessage(String.Format("Radio Button '{0}' = {1}", element.GetKey(), value));
            return timeout;
        }

        #endregion RadioButton

        #region SelectList

        public static int? CustomSelectDropdownItem(this SelectList selectList, string pattern, int? timeout = null, bool runScript = true)
        {
            timeout = selectList.CustomWaitForVisibility(true, timeout);
            string selectedItem = selectList.CustomGetSelectedItem(10);
            if (selectedItem != null)
                selectedItem = selectedItem.Trim();
            if (pattern != selectedItem)
            {
                selectList.Select(new Regex(pattern, RegexOptions.IgnoreCase)); // many selectlist contain preceeding spaces so Regex must be used
                if(runScript)
                    timeout = selectList.CustomRunScript(Util.strChange, null, 0, timeout); // this was change from strKeyUp 12/06/2012
                Util.LogDoneMessage(String.Format("Select '{0}' from dropdown '{1}'.", pattern, selectList.Name));
            }

            return timeout;
        }

        public static int? CustomSelectDropdownItem(this SelectList selectList, int? index = null, int? timeout = null, int min = 1, bool runScript = true)
        {
            timeout = selectList.CustomWaitForVisibility(true, timeout);
            if (!index.HasValue)
                index = Util.GetRandom(min, selectList.Options.Count - 1);
            string txt = selectList.Options[(int)index].CustomGetText();
            return CustomSelectDropdownItem(selectList, txt, timeout, runScript);
        }

        [Obsolete("Use CustomGetSelectedItem", true)]
        public static string GetSelectedListItem(this SelectList selectList)
        {
            Element ee = selectList;
            var items = from item in ee.InnerHtml.Split('/')
                        where item.Contains("selected")
                        select item;

            if (items.FirstOrDefault() != null)
            {
                return items.FirstOrDefault();
            }

            return String.Empty;
        }

        public static string CustomGetSelectedItem(this SelectList selectList, int? timeout = null)
        {
            string item;
            timeout = selectList.CustomWaitForVisibility(timeout: timeout);
            while (timeout > 0 && !selectList.HasSelectedItems)
            {
                Thread.Sleep(1000);
                timeout--;
            }
            if (timeout <= 0)
                item = null;
            else
                try
                {
                    item = selectList.SelectedItem.Trim();
                }
                catch (NullReferenceException)
                {
                    
                    item = string.Empty;
                }
            return item;
        }

        #endregion SelectList

        #region CheckBox

        public static void CustomSetCheckBox(this CheckBox ckbox, bool value = true, int? timeout = null)
        {
            timeout = ckbox.CustomWaitForVisibility(timeout: timeout);
            ckbox.Checked = value;
            Util.Browser.CustomWaitForComplete(timeout);
            Util.LogDoneMessage(String.Format("Check box '{0}' to {1}", ckbox.Name, value));
        }

        public static bool CustomChecked(this CheckBox ckbox, int? timeout = null)
        {
            timeout = ckbox.CustomWaitForVisibility(timeout: timeout);
            return ckbox.Checked;
        }

        #endregion CheckBox

        #region Table

        [Obsolete("There are better ways")]
        public static List<string> GetTableRowsData(this Table table, int rowIndex)
        {
            TableRowCollection tableRows = table.OwnTableRows;

            if (Util.browserType.ToString().Contains("FireFox"))
            {
                TableCellCollection cells = tableRows[rowIndex].OwnTableCells;

                var items = from cell in cells
                            where !string.IsNullOrEmpty(cell.CustomGetText())
                            select cell.CustomGetText();

                return items.ToList();
            }
            else
            {
                // For internet explorer.
                return tableRows[rowIndex].CustomGetText().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToList();
            }
        }

        [Obsolete("There are better ways")]
        public static List<string> GetTableRowsData(this Table table, int tableBodies, int rowIndex)
        {
            TableRowCollection tableRows = table.OwnTableBodies[tableBodies].OwnTableRows;

            if (Util.browserType.ToString().Contains("FireFox"))
            {
                TableCellCollection cells = tableRows[rowIndex].OwnTableCells;

                var items = from cell in cells
                            where !string.IsNullOrEmpty(cell.CustomGetText())
                            select cell.CustomGetText();

                return items.ToList();
            }
            else
            {
                // For internet explorer.
                return tableRows[rowIndex].CustomGetText().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToList();
            }
        }

        [Obsolete("There are better ways")]
        public static List<string> GetTableColumnsData(this Table table, int tableBodies, int rowIndex)
        {
            List<string> columnsData = new List<string>();
            TableRowCollection tableRows = table.OwnTableBodies[tableBodies].OwnTableRows;

            if (Util.browserType.ToString().Contains("FireFox"))
            {
                TableCellCollection cells = tableRows[rowIndex].OwnTableCells;

                var items = from cell in cells
                            where !string.IsNullOrEmpty(cell.CustomGetText())
                            select cell.CustomGetText();

                return items.ToList();
            }
            else
            {
                // For internet explorer.
                for (int index = 0; index < tableRows.Count; index++)
                {
                    columnsData.Add(tableRows[index].CustomGetText().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[rowIndex].Trim());
                }
            }

            return columnsData;
        }

        [Obsolete("There are better ways")]
        public static int GetTableRowsCount(this Table table, int tBodies = 0)
        {
            return table.OwnTableBodies[tBodies].OwnTableRows.Count;
        }

        [Obsolete("There are better ways")]
        public static int FindItem(this Table table, int tBodies, string item)
        {
            int found = -1;

            TableRowCollection tableRows = table.OwnTableBodies[tBodies].OwnTableRows;

            // For internet explorer.
            for (int index = 0; index < tableRows.Count; index++)
            {
                if (tableRows[index].CustomGetText().Contains(item))
                {
                    found = index;
                    break;
                }
            }

            return found;
        }

        #endregion Table

    }
}