using System;

namespace NetSteps.Common.Attributes
{
	public class SortableAttribute : Attribute
	{
		public bool Sortable { get; set; }

		public SortableAttribute(bool sortable)
		{
			Sortable = sortable;
		}
	}
}
