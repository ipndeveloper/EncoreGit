using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NetSteps.Common.Base
{
    public class IndexedListColumns<V> : ConcurrentDictionary<string, Func<V, object>>
    {
        public IndexedListColumns(Dictionary<string, Func<V,object>> dictionary ) : base(dictionary)
        {
            
        }

        public IndexedListColumns(IndexedListColumns<V> dictionary)
            : base(dictionary)
        {

        }

        public IndexedListColumns()
            : base()
        {

        }
    }
}