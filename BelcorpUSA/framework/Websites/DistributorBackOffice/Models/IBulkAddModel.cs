
using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;
namespace DistributorBackOffice.Models
{
	[DTO]
	public interface IBulkAddModel
	{
		string AddProductsUrl { get; set; }
		string GetProductsUrl { get; set; }
		IBulkAddModelData Data { get; set; }
	}
	
	[DTO]
	public interface IBulkAddModelData
	{
		IList<ICategoryInfoModel> Categories { get; set; }
		IList<IBulkProductInfoModel> Products { get; set; }
	}

	[DTO]
	public interface ICategoryInfoModel
	{
		string Name { get; set; }
		int CategoryID { get; set; }
	}

	[DTO]
	public interface IBulkProductInfoModel
	{
		string Name { get; set; }
		int ProductID { get; set; }
		string SKU { get; set; }
		string Price { get; set; }
		int Quantity { get; set; }
	}
}