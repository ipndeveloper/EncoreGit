using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	[DTO]
	public interface IPromotionallyAddedItemModel
	{
		string Description { get; set; }
		string StepID { get; set; }
		IList<IOrderItemModel> Selections { get; set; }
	}
}