using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common
{
    public static class ObjectExtensions
    {
        public static List<T> WrapInList<T>(this T obj)
        {
            return (new T[] { obj }).ToList();
            }
    }
}
