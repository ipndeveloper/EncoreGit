using System;

namespace NetSteps.Common.Attributes
{
	public class DefaultSortAttribute : Attribute
	{
		public Constants.SortDirection SortDirection { get; set; }

		public DefaultSortAttribute(Constants.SortDirection sortDirection)
		{
			SortDirection = sortDirection;
		}
	}
}
