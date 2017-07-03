using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Common.Entities
{
	public interface IOrderItemProperty
	{
		string Name { get; }
		string Value { get; }
	}
}
