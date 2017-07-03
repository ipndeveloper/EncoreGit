using System;
using System.Linq;

namespace NetSteps.Data.Common.Entities
{
    public interface IProduct
    {
        IProductBase ProductBase { get; }
    }
}
