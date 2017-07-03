using System;
using System.Linq;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Services
{
    public interface IProductService
    {
        IProduct Load(int productID);
    }
}
