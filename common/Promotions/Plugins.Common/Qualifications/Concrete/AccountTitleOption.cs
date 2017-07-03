using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class AccountTitleOption : IAccountTitleOption
	{
		public int TitleID { get; set; }

		public int TitleTypeID { get; set; }
	}
}
