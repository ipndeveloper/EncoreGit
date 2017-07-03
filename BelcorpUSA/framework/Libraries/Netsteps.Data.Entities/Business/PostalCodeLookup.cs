using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Tax;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class PostalCodeLookup
    {
        public List<PostalCodeData> GetPostalData(int countryID, string postalCode)
        {
            List<PostalCodeData> returnList = new List<PostalCodeData>();
            //var returnedPostalCodes = Repository.GetPostalCodes(countryID, postalCode);

            var returnedTaxCache = TaxCacheRepository.GetPostalCodes(countryID, postalCode);

            returnList.AddRange(returnedTaxCache.Select(x => new PostalCodeData
            {
                PostalCode = x.PostalCode,
                City = x.City,
                State = x.State,//Province,
                StateAbbreviation = x.StateAbbreviation,//StateProvinceAbbreviation,
                //D01
                //StateID = SmallCollectionCache.Instance.StateProvinces.Where(s => s.StateAbbreviation == x.StateAbbreviation).Select(s => s.StateProvinceID).FirstOrDefault(),
                //A01
                StateID = x.StateProvinceID,
                County = x.County,
                CountryID = (int)x.CountryID,
                Country = x.Street
                //Street = x.Street
            }));

            return returnList;
        }

    }
}
