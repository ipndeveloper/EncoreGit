using System;
using NetSteps.Silverlight.Base;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Silverlight has no type of Session variables since it is client side. This is to substite it for now. - JHE
    /// </summary>
    public static class SessionManager
    {
        public static SessionDictionary Session = new SessionDictionary();
        public class SessionDictionary : ThreadSafeDictionary<string, object>
        {
            public override object this[string key]
            {
                get
                {
                    if (this.List.ContainsKey(key))
                        return this.List[key];
                    else
                        return null;
                }
                set
                {
                    lock (_lock)
                    {
                        this.List[key] = value;
                    }
                }
            }
        }
    }
}
