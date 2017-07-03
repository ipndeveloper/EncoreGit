using System.ComponentModel;

namespace NetSteps.Web.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A base class to add helper methods to access ViewState variables.
    /// Created: 04-18-2009
    /// </summary>
    public class BasicBaseUserControl : System.Web.UI.UserControl
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
            return ((BasicBasePage)Page).GetViewstateVar<T>(name, ViewState, default(T));
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
            return ((BasicBasePage)Page).GetViewstateVar<T>(name, ViewState, defaultValue);
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetViewstateVar<T>(string name, T value)
        {
            ((BasicBasePage)Page).SetViewstateVar<T>(name, ViewState, value);
        }
        #endregion
    }
}
