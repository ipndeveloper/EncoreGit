using System;

namespace NetSteps.Silverlight
{
    public class ExceptionManager
    {
        #region Events
        public event EventHandler<ErrorEventArgs> ErrorOccured;
        public virtual void OnErrorOccured(object sender, ErrorEventArgs e)
        {
            if (ErrorOccured != null)
                ErrorOccured(this, e);
        }
        #endregion

        public void HandleError(Exception ex)
        {
            OnErrorOccured(null, new ErrorEventArgs(ex));
        }

        public void HandleError(ErrorEventArgs ex)
        {
            OnErrorOccured(null, ex);
        }
    }
}
