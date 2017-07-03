using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
	public interface IAccountHasTitleQualificationExtension : IPromotionQualificationExtension
	{
		IList<IAccountTitleOption> AllowedTitles { get; }
	}
}
