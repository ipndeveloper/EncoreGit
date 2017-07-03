using System;
using System.Linq.Expressions;

namespace NetSteps.Data.Entities.Business.Filters.Interfaces
{
	public interface IUrlSelector
	{
		string Url { get; set; }
		bool IsValid { get; set; }
		Expression<Func<SiteUrl, bool>> Filter { get; }
	}
}
