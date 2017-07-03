﻿using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountPropertyTypeRepository
    {
        List<AccountPropertyType> LoadAllFullAccountPropertyType();
        PaginatedList<AccountPropertyType> Search(FilterPaginatedListParameters<AccountPropertyType> searchParams);
    }
}
