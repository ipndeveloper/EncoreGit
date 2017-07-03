using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	[DTO]
	public interface IKitItemsModel
	{
		IList<IKitItemModel> KitItemModels { get; set; }
	}
}