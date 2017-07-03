using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IBrandRepository
    {
        Brand Load(string brandNumber);
        IList<Brand> GetAll();
    }
}
