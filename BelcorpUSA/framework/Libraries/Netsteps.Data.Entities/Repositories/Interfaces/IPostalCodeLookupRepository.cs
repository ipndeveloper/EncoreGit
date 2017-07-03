using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPostalCodeLookupRepository
    {
        List<PostalCodeLookup> GetPostalCodes(int countryID, string postalCode);
    }
}
