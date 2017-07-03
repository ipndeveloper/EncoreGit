using System;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 12/17/2009
    /// </summary>
    public class StateManager
    {
        #region Events
        public event StateEventHandler StateChanged;
        protected virtual void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            if (StateChanged != null)
                StateChanged(this, e);
        }
        #endregion

        public void SendStateChange(object sender, StateChangedEventArgs e)
        {
            OnStateChanged(sender, e);
        }
    }
}
