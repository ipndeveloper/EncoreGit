using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IStateProvinceRepository
    {
        List<StateProvince> LoadStatesByCountry(int countryID);
    }
}
