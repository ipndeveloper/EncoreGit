using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetSteps.Web.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A base class to add helper methods to access ViewState variables.
    /// Created: 06-17-2009
    /// </summary>
    public class BasicWebControl : WebControl
    {
        #region Members
        protected static string javascriptTemplate = "\n<script type=\"text/javascript\"> {0} </script>";
        #endregion

        #region Properties
        [Description("Unique GUID to use referencening the control as needed.")]
        protected string ControlGuid
        {
            get
            {
                if (GetViewstateVar<string>("ControlGuid", string.Empty) == string.Empty)
                    ControlGuid = System.Guid.NewGuid().ToString();
                return GetViewstateVar<string>("ControlGuid", string.Empty);
            }
            set { SetViewstateVar<string>("ControlGuid", value); }
        }
        #endregion

        #region Viewstate Variable Methods
        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name)
        {
            return GetViewstateVar<T>(name, default(T));
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name, T defaultValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (ViewState[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, ViewState[name].ToString());
                        else
                            result = (T)Convert.ChangeType(ViewState[name], objectType);
                        return result;
                    }
                }
                return defaultValue;
            }
            catch
            {
                // Error Probably caused by bad URL data (returning default data value) - JHE
                return defaultValue;
            }
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetViewstateVar<T>(string name, object value)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    object obj = (T)result;
                    if (obj is Enum)
                        ViewState[name] = (T)Enum.Parse(objectType, value.ToString());
                    else
                        ViewState[name] = (T)value;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// This method guatantees that only one instance of the script get included in the page header. - JHE
        /// </summary>
        /// <param name="key"></param>
        /// <param name="javascript"></param>
        protected void RegisterClientScriptOnHeader(string key, string javascript)
        {
            if (this.Page.Header == null)
                throw new Exception("Missing runat=server attribute on <head> tag.");

            if (this.Page.Header.FindControl(key) == null)
            {
                LiteralControl literal = new LiteralControl(String.Format(javascriptTemplate, "\n" + javascript + "\n") + "\n");
                literal.ID = key;
                this.Page.Header.Controls.Add(literal);
            }
        }
        #endregion
    }
}
