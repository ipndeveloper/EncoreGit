
namespace NetSteps.Common.Validation
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A basic class for encapsulating a validation error message
    /// Created: 03-05-2010
    /// </summary>
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public string FullErrorMessage
        {
            get
            {
                return string.Format("{0}: {1}", PropertyName, ErrorMessage);
            }
        }
    }
}
