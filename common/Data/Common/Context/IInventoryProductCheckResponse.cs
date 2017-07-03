using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Common.Context
{
	public interface IInventoryProductCheckResponse
	{
		int CanAddNormally { get; }
		int CanAddBackorder { get; }
		int CannotAddOutOfStock { get; }
		int CannotAddInvalid { get; }
		int CannotAddInactive { get; }
	}
}
