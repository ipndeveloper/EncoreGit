using System;
using System.Configuration;
using NetSteps.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiPurchaseOrderConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty WarehouseIdProperty = new ConfigurationProperty("warehouseId", typeof(string));
		public string WarehouseId
		{
			get { return (string)this[WarehouseIdProperty]; }
			set { this[WarehouseIdProperty] = value; }
		}

		public static readonly ConfigurationProperty ShipFromNameProperty = new ConfigurationProperty("shipFromName", typeof(string), String.Empty);
		public string ShipFromName
		{
			get { return (string)this[ShipFromNameProperty]; }
			set { this[ShipFromNameProperty] = value; }
		}

		public static readonly ConfigurationProperty ShipFromIdentificationCodeProperty = new ConfigurationProperty("shipFromIdentificationCode", typeof(string), "U21");
		public string ShipFromIdentificationCode
		{
			get { return (string)this[ShipFromIdentificationCodeProperty]; }
			set { this[ShipFromIdentificationCodeProperty] = value; }
		}

		public static readonly ConfigurationProperty ShipFromIdentificationQualifierProperty = new ConfigurationProperty("shipFromIdentificationQualifier", typeof(string), "91");
		public string ShipFromIdentificationQualifier
		{
			get { return (string)this[ShipFromIdentificationQualifierProperty]; }
			set { this[ShipFromIdentificationQualifierProperty] = value; }
		}

		public static readonly ConfigurationProperty PaidStatusCodeProperty = new ConfigurationProperty("paidStatusCode", typeof(string), "VM");
		public string PaidStatusCode
		{
			get { return (string)this[PaidStatusCodeProperty]; }
			set { this[PaidStatusCodeProperty] = value; }
		}

		public static readonly ConfigurationProperty CanceledStatusCodeProperty = new ConfigurationProperty("canceledStatusCode", typeof(string), "AB");
		public string CanceledStatusCode
		{
			get { return (string)this[CanceledStatusCodeProperty]; }
			set { this[CanceledStatusCodeProperty] = value; }
		}

		public static readonly ConfigurationProperty ReturnedStatusCodeProperty = new ConfigurationProperty("returnedStatusCode", typeof(string), "RT");
		public string ReturnedStatusCode
		{
			get { return (string)this[ReturnedStatusCodeProperty]; }
			set { this[ReturnedStatusCodeProperty] = value; }
		}
	}

	public class EdiPurchaseOrderCollectionElement : GenericConfigurationElementCollection<EdiPurchaseOrderConfigurationElement>
	{
		public EdiPurchaseOrderCollectionElement()
			: base((i) => i.WarehouseId) { }
	}
}
