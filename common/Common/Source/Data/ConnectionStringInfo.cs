
namespace NetSteps.Common.Data
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to help parse a connection string. 
    /// Created: 11/17/2010
    /// </summary>
    public class ConnectionStringInfo
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ApplicationName { get; set; }
        public string MultipleActiveResultSets { get; set; }
        public string MaxPoolSize { get; set; }
    }
}
