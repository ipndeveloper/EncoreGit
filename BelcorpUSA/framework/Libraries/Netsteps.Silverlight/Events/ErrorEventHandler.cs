using System;
using NetSteps.Silverlight.Exceptions;

namespace NetSteps.Silverlight
{
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    public class ErrorEventArgs : EventArgs
    {
        public string UserControl { get; set; }
        public Exception Exception { get; set; }

        public ErrorEventArgs(Exception exception)
        {
            if (!(exception is GeneralException))
                Exception = new GeneralException(exception);
            else
                Exception = exception;
        }

        public ErrorEventArgs(Exception exception, string userControl)
        {
            if (!(exception is GeneralException))
                Exception = new GeneralException(exception);
            else
                Exception = exception;

            UserControl = userControl;
        }

        public ErrorEventArgs(Exception exception, string userControl, string extraInfo)
        {
            if (!(exception is GeneralException))
                Exception = new GeneralException(exception, extraInfo);
            else
                Exception = exception;

            UserControl = userControl;
        }
    }
}