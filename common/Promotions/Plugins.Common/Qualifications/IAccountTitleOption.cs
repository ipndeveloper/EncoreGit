using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
	public interface IAccountTitleOption
	{
		int TitleID { get; set; }
		int TitleTypeID { get; set; }
	}
}
