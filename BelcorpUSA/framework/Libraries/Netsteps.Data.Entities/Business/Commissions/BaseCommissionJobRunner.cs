using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Commissions
{

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class BaseCommissionJobRunner
	{


		#region Members

		private ICommissionsUiRepository _repository;

		#endregion

		#region Constructor

		public BaseCommissionJobRunner()
			: this(null)
		{
		}

		public BaseCommissionJobRunner(ICommissionsUiRepository commissionsRepo)
		{
			_repository = commissionsRepo ?? Create.New<ICommissionsUiRepository>();
		}

		#endregion

		#region Public Methods



		#endregion

		#region Protected Methods



		#endregion

	}
}
