using System;
using NetSteps.Common.Base;

namespace NetSteps.Common.Extensions
{
    public static class CachedListBusinessExtensions
    {
        public static T GetByIdOrNew<T, TKeyType>(this CachedListBusiness<T, TKeyType> list, TKeyType id)
            where T : new()
            where TKeyType : IComparable
        {
            T item = list.GetById(id);

            if (item == null || item.Equals(default(T)))
                item = new T();

            return item;
        }
    }
}
