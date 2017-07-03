using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICountryRepository
    {
        List<Country> GetCountries();
    }
}
