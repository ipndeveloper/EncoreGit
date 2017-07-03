using System.Collections.Generic;

namespace nsCore.Areas.Commissions.Models
{
	public class EarningsPerPeriodModel
	{

		#region Constructors

        public EarningsPerPeriodModel()
		{
			//For default model binding.
		}

		#endregion

		#region Properties

		public Dictionary<string,string> Periods { get; set; }
        public Dictionary<string, string> BonusTypes { get; set; }

		#endregion

		#region Methods

		#endregion

	}
}