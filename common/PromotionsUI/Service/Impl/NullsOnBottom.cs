using System;
using System.Collections.Generic;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class NullsOnBottom : IComparer<DateTime?>
    {
        public int Compare(DateTime? x, DateTime? y)
        {
            if (x == null && y != null) return 1;
            if (x != null && y == null) return -1;
            return Comparer<DateTime?>.Default.Compare(x, y);
        }
    }
}
