using NetSteps.Encore.Core.Dto;

namespace NetSteps.Orders.UI.Common.Models
{
	[DTO]
	public interface IOrderDetailItemMessageModel
	{
		string Message { get; set; }
		int MessageKind { get; set; }
	}
}
