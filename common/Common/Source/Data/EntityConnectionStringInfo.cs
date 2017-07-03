
namespace NetSteps.Common.Data
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to help parse a connection string. 
    /// Created: 03/14/2011
    /// </summary>
    public class EntityConnectionStringInfo
    {
        public string Metadata { get; set; }
        public string Provider { get; set; }
        public ConnectionStringInfo ProviderConnectionString { get; set; }
    }
}
