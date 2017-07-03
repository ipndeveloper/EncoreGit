using WatiN.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class SearchSuggestionBox_Control : Control<TextField>
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();     
        }

        /// <summary>
        /// Search & select from suggestions control
        /// </summary>
        /// <param name="search">search string</param>
        /// <param name="select">select string. If null then use search string</param>
        /// <param name="exactMatch">exact match</param>
        /// <param name="timeout"></param>
        /// <param name="jQueryIndex"></param>
        /// <param name="jQuery"></param>
        /// <returns></returns>
        public bool Select(string search, string select = null, bool exactMatch = false, int? timeout = null, int jQueryIndex = 0, string jQuery = null)
        {
            bool success = false;

            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;

            Element.CustomSetTextHelper(search);
            Thread.Sleep(1000);

            if (jQuery == null)
            {
                Element.CustomRunScript(Util.strKeyUp, null, jQueryIndex, timeout);
            }
            else
            {
                Util.CustomRunScript(jQuery, timeout);
            }

            Element nextSibling;
            do
            {
                Thread.Sleep(1000);
                nextSibling = Element.NextSibling;
                if (nextSibling.GetType() == typeof(Div) && nextSibling.IsVisible())
                    break;
            } while (timeout-- > 0);

            Div results = (Div)nextSibling;

            ElementCollection<Div> jsonResults;
            do
            {             
                jsonResults = results.ElementsOfType<Div>();
                if (jsonResults.Count > 0 && jsonResults[0].Exists && !jsonResults[0].CustomGetText(timeout).Contains("loading"))
                        break;
                Thread.Sleep(1000);
            }while (--timeout > 0);
            if (timeout > 0)
            {
                if (select == null)
                    select = search;
                jsonResults = results.ElementsOfType<Div>();
                Regex regex = new Regex(select, RegexOptions.IgnoreCase);
                for (int i = 0; i < jsonResults.Count; i++)
                {
                    if ((exactMatch == true && jsonResults[i].CustomGetText() == select) || (exactMatch == false && regex.IsMatch(jsonResults[i].CustomGetText())))
                    {
                        success = true;
                        jsonResults[i].CustomClick(timeout);
                        break;
                    }
                }
            }
            else
            {
                throw new TimeoutException();
            }
            return success;            
        }

        [Obsolete("Use 'Select(string, string = null, int? = null, int = 0, string = null)'", true)]
        public bool Select(string firstName, string lastName, string userId)
        {
            return Select(string.Format("{0} {1} (#{2})", firstName, lastName, userId));
        }

        public string Text
        {
            get { return Element.CustomGetText(); }
        }
    }
}
