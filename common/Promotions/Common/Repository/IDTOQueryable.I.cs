using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Common.Repository
{
    public interface IDTOQueryable<I> : IQueryable<I>
    {
        void AddObject(I dto);
    }
}
