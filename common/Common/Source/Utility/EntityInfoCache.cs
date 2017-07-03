using System;
using System.Reflection;
using System.Collections.Concurrent;

namespace NetSteps.Common
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to cache some data for use with Entity Framework.
    /// Created: 03-12-2010
    /// </summary>
    public class EntityInfoCache
    {
        public static ConcurrentDictionary<Type, PrimaryKeyInfo> EntityPrimaryKeys = new ConcurrentDictionary<Type, PrimaryKeyInfo>();
        public static ConcurrentDictionary<Type, string> EntitySetNames = new ConcurrentDictionary<Type, string>();
    }

    public class PrimaryKeyInfo
    {
        public string ColumnName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
