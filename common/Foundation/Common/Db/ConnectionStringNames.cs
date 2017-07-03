using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// Contains the connection string names for the Encore databases.
    /// Enables components to share the same connection strings.
    /// </summary>
    public static class ConnectionStringNames
    {
        /// <summary>
        /// The connection string name for the Core database.
        /// </summary>
        public static string Core { get { return "Core"; } }
        
        /// <summary>
        /// The connection string name for the Commissions database.
        /// </summary>
        public static string Commissions { get { return "Commissions"; } }
        
        /// <summary>
        /// The connection string name for the Mail database.
        /// </summary>
        public static string Mail { get { return "Mail"; } }
    }
}
