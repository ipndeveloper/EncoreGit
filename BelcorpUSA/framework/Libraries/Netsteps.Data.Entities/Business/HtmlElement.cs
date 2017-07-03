using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities
{
	public partial class HtmlElement : ISortIndex
	{
		#region Methods
		public string BuildElement(bool wrap = true)
		{
			return BusinessLogic.BuildElement(this, wrap);
		}
		#endregion

		#region ISortIndex Members

		int ISortIndex.SortIndex
		{
			get
			{
				return SortIndex;
			}
			set
			{
				SortIndex = value.ToByte();
			}
		}

		#endregion
	}
}
