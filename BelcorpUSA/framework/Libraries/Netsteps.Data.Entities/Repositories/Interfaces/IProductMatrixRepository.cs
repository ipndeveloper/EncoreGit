using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IProductMatrixRepository
    {
        PaginatedList<ProductMatrix> GetMatrixErrorLog(ProductMatrixParameters searchParams);
    }
}
