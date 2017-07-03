using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace nsCore.Areas.Orders.Models
{
	[DTO]
	public interface IKitItemsModel
	{
		IList<IKitItemModel> KitItemModels { get; set; }
	}
}
