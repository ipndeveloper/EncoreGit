using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class PostalCodeLookupRepository
    {
        public List<PostalCodeLookup> GetPostalCodes(int countryID, string postalCode)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.PostalCodeLookups.Where(x => x.CountryID == countryID && x.PostalCode == postalCode).ToList();
                }
            });
        }
    }
}
