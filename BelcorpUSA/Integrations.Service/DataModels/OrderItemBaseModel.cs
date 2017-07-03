using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
	[DataContract]
	public class OrderItemBaseModel
	{
		/// <summary>
		/// Gets or sets the item identifier.  This correlates to OrderItemID at present.
		/// </summary>
		/// <value>
		/// The item identifier.
		/// </value>
		[DataMember(Name = "itemID", IsRequired = true)]
		public int ItemID { get; set; }

		/// <summary>
		/// Gets or sets the sku.
		/// </summary>
		/// <value>
		/// The sku.
		/// </value>
		[DataMember(Name = "sku", IsRequired = true)]
		public string SKU { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>
		/// The quantity.
		/// </value>
		[DataMember(Name = "quantity", IsRequired = true)]
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the unit price.
		/// </summary>
		/// <value>
		/// The unit price.
		/// </value>
		[DataMember(Name = "unitPrice", IsRequired = true)]
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// Gets or sets the item total.
		/// </summary>
		/// <value>
		/// The item total.
		/// </value>
		[DataMember(Name = "itemTotal", IsRequired = true)]
		public decimal ItemTotal { get; set; }

		#region Belcorp values

		/// <summary>
		/// Gets or sets the material group identifier. This is a number specific to Belcorp.
		/// </summary>
		/// <value>
		/// The material group identifier.
		/// </value>
		[DataMember(Name = "materialGroupID", IsRequired = false)]
		public string MaterialGroupID { get; set; }

		/// <summary>
		/// Gets or sets the SAP code.
		/// </summary>
		/// <value>
		/// The SAP code.
		/// </value>
		[DataMember(Name = "sapCode", IsRequired = true)]
		public string SAPCode { get; set; }

		/// <summary>
		/// Gets or sets the SAP code.
		/// </summary>
		/// <value>
		/// The SAP code.
		/// </value>
		[DataMember(Name = "offerType", IsRequired = true)]
		public string OfferType { get; set; }

		/// <summary>
		/// Gets or sets the type of the product.  Values are set by Belcorp.
		/// </summary>
		/// <value>
		/// The type of the product.
		/// </value>
		[DataMember(Name = "productType", IsRequired = true)]
		public int ProductType { get; set; }

		/// <summary>
		/// Gets or sets the type of the sale.
		/// </summary>
		/// <value>
		/// The type of the sale.
		/// </value>
		[DataMember(Name = "saleType", IsRequired = true)]
		public int SaleType { get; set; }

		#endregion
	}

	[DataContract(Name = "saleType")]
	public enum InvoiceMaterialGroupSaleType
	{
		[EnumMember]
		Unknown = 0,
		[EnumMember]
		Regular = 1,
		[EnumMember]
		Gift = 2
	}

	[DataContract(Name = "productType")]
	public enum InvoiceMaterialGroupProductType
	{
		[EnumMember]
		Unknown = 0,
		[EnumMember]
		Regular = 1,
		[EnumMember]
		Incentive = 2,
		[EnumMember]
		EnrollmentItem = 3,
		[EnumMember]
		GlamUp = 4,
		[EnumMember]
		RecruitingSample = 5,
		[EnumMember]
		ConferenceSample = 6,
		[EnumMember]
		LegalSample = 7,
		[EnumMember]
		MarketSample = 8,
		[EnumMember]
		EventSample = 9,
		[EnumMember]
		RegistrationSample = 10
	}
}
