using System;

namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface to provide A generic way to list objects (read only).
    /// Created: 05-06-2010
    /// </summary>
    public interface IKeyName<T, TKeyType>
    {
        TKeyType ID { get; set; }
        string Title { get; set; }

        Func<T, TKeyType> GetIdColumnFunc { get; }
        Action<T, TKeyType> SetIdColumnFunc { get; }
        Func<T, string> GetTitleColumnFunc { get; }
        Action<T, string> SetTitleColumnFunc { get; }
    }
}
