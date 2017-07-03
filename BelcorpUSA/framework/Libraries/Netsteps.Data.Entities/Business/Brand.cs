using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public partial class Brand
    {
        public static IList<Brand> All()
        {
            return Repository.GetAll();
        }

        public static Brand Load(string number)
        {
            return Repository.Load(number);
        }
    }
}
