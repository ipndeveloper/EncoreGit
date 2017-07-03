using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ShippingMethodRepository
    {
        protected override Func<NetStepsEntities, IQueryable<ShippingMethod>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<ShippingMethod>>(
                 (context) => context.ShippingMethods.Include("Translations"));
            }
        }

        protected override Func<NetStepsEntities, int, IQueryable<ShippingMethod>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<ShippingMethod>>(
                 (context, shippingMethodID) => context.ShippingMethods.Include("Translations")
                                               .Where(sm => sm.ShippingMethodID == shippingMethodID));
            }
        }

        public List<int> LoadAllTranslationIds()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<int> ids = new List<int>();
                    var shippingMethods = (from s in context.ShippingMethods
                                                    .Include("Translations")
                                           select s).ToList();

                    foreach (var shippingMethod in shippingMethods)
                        foreach (var translation in shippingMethod.Translations)
                            ids.Add(translation.DescriptionTranslationID);

                    return ids;
                }
            });
        }

		public ShippingMethod LoadByName(string name)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var result = context.ShippingMethods.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

					if (result != null)
					{
						result.StartEntityTracking();
					}

					return result;
				}
			});
		}
    }
}
