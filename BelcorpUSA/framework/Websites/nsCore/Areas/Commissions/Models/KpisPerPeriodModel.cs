﻿using System.Collections.Generic;

namespace nsCore.Areas.Commissions.Models
{
	public class KpisPerPeriodModel
	{

		#region Constructors

        public KpisPerPeriodModel()
		{
			//For default model binding.
		}

		#endregion

		#region Properties

		public Dictionary<string,string> Periods { get; set; }

		#endregion

		#region Methods

		#endregion

	}
}