using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Common.Context
{
	[DTO]
	public interface IOrderItemQuantityModification : IOrderItemModification
	{
		OrderItemQuantityModificationKind ModificationKind { get; set; }
		int Quantity { get; set; }
	}

	public enum OrderItemQuantityModificationKind
	{
		Add,
		Delete,
		SetToQuantity
	}
}
