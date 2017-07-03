using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace NetSteps.Web.Controls.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A base class to add helper methods to access ViewState variables.
    /// Created: 06-08-2009
    /// </summary>
    public class BasicCompositeControl : CompositeControl
    {
        [Description("Unique GUID to use referencing the control as needed.")]
        public string ControlGuid
        {
            get
            {
                if (GetViewstateVar<string>("ControlGuid", string.Empty) == string.Empty)
                    ControlGuid = System.Guid.NewGuid().ToString();
                return GetViewstateVar<string>("ControlGuid", string.Empty);
            }
            set { SetViewstateVar<string>("ControlGuid", value); }
        }

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
                            result = (T)Convert.ChangeType(ViewState[name].ToString(), objectType);
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
    }
}
