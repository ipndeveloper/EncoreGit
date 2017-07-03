using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Common.Entities
{
	[DTO]
	public interface IPriceType
	{
		string Name { get; set; }
		string TermName { get; set; }
		int PriceTypeID { get; set; }
	}
}
