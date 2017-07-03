using System.Collections.Generic;

namespace nsCore.Areas.Commissions.Models
{
	public class JobOutputModel
	{

		#region Constructors

		public JobOutputModel()
		{
			//For default model binding.
		}

		#endregion

		#region Properties

		public List<string> JobStatuses { get; set; }

		#endregion

		#region Methods

		#endregion

	}
}