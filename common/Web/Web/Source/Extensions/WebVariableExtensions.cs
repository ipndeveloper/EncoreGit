using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Generic helper methods to get variables from Session, QueryString, and Attributes
    /// with parameters to specify a default if the variable does not exist.
    /// Created: 03-18-2009
    /// </summary>
    public static class WebVariableExtensions
    {
        #region Session Variable Methods
        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSessionVar<T>(this Control value, string name)
        {
            return value.GetSessionVar<T>(name, default(T), null);
        }

        public static T GetSessionVar<T>(this Control value, string name, T defaultValue)
        {
            return value.GetSessionVar<T>(name, defaultValue, null);
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetSessionVar<T>(this Control value, string name, T defaultValue, Func<T> instanciate)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (HttpContext.Current.Session[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, HttpContext.Current.Session[name].ToString());
                        else
                            result = (T)Convert.ChangeType(HttpContext.Current.Session[name], objectType);

                        return result;
                    }
                    else
                    {
                        if (instanciate != null)
                            defaultValue = instanciate();
                    }
                }
                HttpContext.Current.Session[name] = defaultValue;
                return defaultValue;
            }
            catch
            {
                // Error Probably caused by bad URL data (returning default data value) - JHE
                HttpContext.Current.Session[name] = defaultValue;
                return defaultValue;
            }
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetSessionVar<T>(this Control value, string name, T variableValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    object obj = (T)result;
                    if (obj is Enum)
                        HttpContext.Current.Session[name] = (T)Enum.Parse(objectType, variableValue.ToString());
                    else
                        HttpContext.Current.Session[name] = (T)variableValue;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region QueryString Variable Methods
        public static T GetQueryStringVar<T>(this System.Web.UI.Page value, string name)
        {
            return value.GetQueryStringVar<T>(name, default(T));
        }
        public static T GetQueryStringVar<T>(this System.Web.UI.Page value, string name, T defaultValue)
        {
            return GetQueryStringVar<T>(name, defaultValue);
        }

        public static T GetQueryStringVar<T>(this System.Web.UI.UserControl value, string name)
        {
            return value.GetQueryStringVar<T>(name, default(T));
        }
        public static T GetQueryStringVar<T>(this System.Web.UI.UserControl value, string name, T defaultValue)
        {
            return GetQueryStringVar<T>(name, defaultValue);
        }

        public static T GetQueryStringVar<T>(this System.Web.UI.WebControls.WebControl value, string name)
        {
            return value.GetQueryStringVar<T>(name, default(T));
        }
        public static T GetQueryStringVar<T>(this System.Web.UI.WebControls.WebControl value, string name, T defaultValue)
        {
            return GetQueryStringVar<T>(name, defaultValue);
        }

        public static T GetQueryStringVar<T>(this System.Web.UI.MasterPage value, string name)
        {
            return value.GetQueryStringVar<T>(name, default(T));
        }
        public static T GetQueryStringVar<T>(this System.Web.UI.MasterPage value, string name, T defaultValue)
        {
            return GetQueryStringVar<T>(name, defaultValue);
        }

        private static T GetQueryStringVar<T>(string name, T defaultValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (HttpContext.Current.Request.QueryString[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, HttpContext.Current.Request.QueryString[name].ToString());
                        else if (obj is Boolean)
                        {
                            string boolValue = HttpContext.Current.Request.QueryString[name].ToString().ToLower();
                            if (boolValue.ToBool())
                                result = (T)Convert.ChangeType(true, objectType);
                            else
                                result = (T)Convert.ChangeType(false, objectType);
                        }
                        else
                            result = (T)Convert.ChangeType(HttpContext.Current.Request.QueryString[name].ToString(), objectType);
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
        #endregion

        #region Attribute Variable Methods
        public static T GetAttributeValue<T>(this WebControl value, string name)
        {
            return GetAttributeValue<T>(value, name, default(T));
        }

        public static T GetAttributeValue<T>(this WebControl value, string name, T defaultValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (value.Attributes[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, value.Attributes[name].ToString());
                        else
                            result = (T)Convert.ChangeType(value.Attributes[name].ToString(), objectType);
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
        #endregion
    }
}
