using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class TaxCacheBusinessLogic
	{
		public override void CleanDataBeforeSave(Repositories.ITaxCacheRepository repository, TaxCache entity)
		{
			entity.PostalCode = entity.PostalCode.ToCleanStringNullable();
			entity.State = entity.State.ToCleanStringNullable();
			entity.StateAbbreviation = entity.StateAbbreviation.ToCleanStringNullable();
			entity.City = entity.City.ToCleanStringNullable();
			entity.County = entity.County.ToCleanStringNullable();
		}
	}
}
