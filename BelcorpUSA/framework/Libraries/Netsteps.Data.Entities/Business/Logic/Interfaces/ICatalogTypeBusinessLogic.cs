using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ICatalogTypeBusinessLogic
    {
        IEnumerable<int> GetShoppableCatalogTypeIds();
    }
}
