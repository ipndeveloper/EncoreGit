using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class StateProvinceRepository
	{
		#region Members
		#endregion

		public List<StateProvince> LoadStatesByCountry(int countryID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.StateProvinces.Where(sp => sp.CountryID == countryID).ToList();
				}
			});
		}
	}
}
