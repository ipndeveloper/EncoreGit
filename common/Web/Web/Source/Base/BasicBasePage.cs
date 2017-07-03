using System;
using System.Web.UI;

namespace NetSteps.Web.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A base class to add helper methods to access ViewState variables.
    /// Created: 04-18-2009
    /// </summary>
    public class BasicBasePage : System.Web.UI.Page
    {
        #region Viewstate Variable Methods
        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name)
        {
            return GetViewstateVar<T>(name, ViewState, default(T));
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name, T defaultValue)
        {
            return GetViewstateVar<T>(name, ViewState, defaultValue);
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="stateBag">Pass in the ViewState for the current control.</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetViewstateVar<T>(string name, StateBag stateBag, T defaultValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (stateBag[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, stateBag[name].ToString());
                        else
                            result = (T)Convert.ChangeType(stateBag[name].ToString(), objectType);

                        return result;
                    }
                }
                stateBag[name] = defaultValue;
                return defaultValue;
            }
            catch
            {
                // Error Probably caused by bad URL data (returning default data value) - JHE
                stateBag[name] = defaultValue;
                return defaultValue;
            }
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetViewstateVar<T>(string name, T value)
        {
            SetViewstateVar<T>(name, ViewState, value);
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetViewstateVar<T>(string name, StateBag stateBag, T value)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    object obj = (T)result;
                    if (obj is Enum)
                        stateBag[name] = (T)Enum.Parse(objectType, value.ToString());
                    else
                        stateBag[name] = (T)value;
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}
