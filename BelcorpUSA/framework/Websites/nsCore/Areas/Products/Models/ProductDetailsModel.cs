using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Products.Models
{
	public class ProductDetailsModel
	{
		public Product Product { get; set; }
		public string Sku { get; private set; }
		public int ProductTypeId { get; set; }
		public int? TaxCategoryId { get; set; }
		public short BackOrderBehaviorId { get; set; }
		public double? Weight { get; set; }
		public bool ChargeShipping { get; set; }
		public bool ChargeTax { get; set; }
		public bool ChargeTaxOnShipping { get; set; }
		public bool DisplayChargeTaxOnShipping { get; set; }
		public bool IsShippable { get; set; }
		public List<int> ExcludedShippingMethodIds { get; set; }
		public List<ShippingMethod> AvailableShippingMethods { get; set; }
		public bool UpdateInventoryOnBase { get; set; }
		public bool? ShowKitContents { get; set; }
		public TrackableCollection<DescriptionTranslation> Translations { get; set; }
		public TrackableCollection<ProductRelation> ChildProductRelations { get; set; }

		public ProductDetailsModel()
		{ }

		public ProductDetailsModel(Product product)
		{
			product = Product.LoadFull(product.ProductID);
			Product = product;
			Sku = product.SKU;
			ProductTypeId = product.ProductBase.ProductTypeID;
			TaxCategoryId = product.ProductBase.TaxCategoryID;
			BackOrderBehaviorId = product.ProductBackOrderBehaviorID;
			Weight = product.Weight;
			ChargeShipping = product.ProductBase.ChargeShipping;
			ChargeTax = product.ProductBase.ChargeTax;
			ChargeTaxOnShipping = product.ProductBase.ChargeTaxOnShipping;
			DisplayChargeTaxOnShipping = true;
			IsShippable = product.ProductBase.IsShippable;
			UpdateInventoryOnBase = product.ProductBase.UpdateInventoryOnBase;
			ShowKitContents = product.ShowKitContents;
			Translations = product.Translations;
		}
	}
}
