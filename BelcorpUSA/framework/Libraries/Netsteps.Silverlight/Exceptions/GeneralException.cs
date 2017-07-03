using System;

namespace NetSteps.Silverlight.Exceptions
{
    /// <summary>
    /// This class will wrap all other exceptions to support better logging and not log the same error more than once. - JHE
    /// </summary>
    public class GeneralException : Exception
    {
        public bool HasBeenLogged { get; set; }
        public string ExtraInfo { get; set; }

        public GeneralException()
        {
        }

        public GeneralException(Exception ex)
            : base(ex.Message, ex)
        {
        }

        public GeneralException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public GeneralException(Exception ex, string extraInfo)
            : base(ex.Message, ex)
        {
            ExtraInfo = extraInfo;
        }
    }

    public class WcfCallException : GeneralException
    {
        public string ServiceCall { get; set; }

        public WcfCallException(Exception ex, string serviceCall)
            : base(ex.Message, ex)
        {
            ServiceCall = serviceCall;
        }
    }
}
