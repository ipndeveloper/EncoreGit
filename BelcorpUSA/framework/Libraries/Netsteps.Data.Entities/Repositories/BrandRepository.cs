using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class BrandRepository
    {
        public Brand Load(string brandNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Brands.Include("DescriptionTranslations").FirstOrDefault(b => b.BrandNumber == brandNumber);
                }
            });
        }

        public IList<Brand> GetAll()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Brands.ToList();
                }
            });
        }
    }
}
