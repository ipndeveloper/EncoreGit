using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Configuration;
using System.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiInterchangeConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty OrderCancellationGracePeriodProperty = new ConfigurationProperty("orderCancellationGracePeriod", typeof(TimeSpan), "0.01:00:00.0");
		public TimeSpan OrderCancellationGracePeriod
		{
			get { return (TimeSpan)this[OrderCancellationGracePeriodProperty]; }
			set { this[OrderCancellationGracePeriodProperty] = value; }
		}

		public static readonly ConfigurationProperty PromotionItemPaidCMTCodeProperty = new ConfigurationProperty("promotionItemPaidCMTCode", typeof(String), "S13");
		public string PromotionItemPaidCMTCode
		{
			get { return (string)this[PromotionItemPaidCMTCodeProperty]; }
			set { this[PromotionItemPaidCMTCodeProperty] = value; }
		}

		public static readonly ConfigurationProperty PromotionItemReturnCMTCodeProperty = new ConfigurationProperty("promotionItemReturnCMTCode", typeof(String), "S14");
		public string PromotionItemReturnCMTCode
		{
			get { return (string)this[PromotionItemReturnCMTCodeProperty]; }
			set { this[PromotionItemReturnCMTCodeProperty] = value; }
		}

		public static readonly ConfigurationProperty PurchaseOrdersProperty = new ConfigurationProperty("purchaseOrders", typeof(EdiPurchaseOrderCollectionElement));
		public EdiPurchaseOrderCollectionElement PurchaseOrders
		{
			get { return (EdiPurchaseOrderCollectionElement)this[PurchaseOrdersProperty]; }
			set { this[PurchaseOrdersProperty] = value; }
		}

		public static readonly ConfigurationProperty PartnerInterchangeIdsProperty = new ConfigurationProperty("partners", typeof(EdiPartnerInterchangeIdCollectionElement));
		public EdiPartnerInterchangeIdCollectionElement PartnerInterchangeIds
		{
			get { return (EdiPartnerInterchangeIdCollectionElement)this[PartnerInterchangeIdsProperty]; }
			set { this[PartnerInterchangeIdsProperty] = value; }
		}

		public class EdiPartnerInterchangeIdCollectionElement : GenericConfigurationElementCollection<EdiPartnerInterchangeIdConfigurationElement>
		{
			public EdiPartnerInterchangeIdCollectionElement()
				: base((i) => i.PartnerName) { }
		}

		public class EdiPartnerInterchangeIdConfigurationElement : DeclaredMemberConfigurationElement
		{
			public static readonly ConfigurationProperty PartnerNameProperty = new ConfigurationProperty("partnerName", typeof(string));
			public string PartnerName
			{
				get { return (string)this[PartnerNameProperty]; }
				set { this[PartnerNameProperty] = value; }
			}

			public static readonly ConfigurationProperty PartnerTimeZoneProperty = new ConfigurationProperty("timeZone", typeof(string), "Pacific Standard Time");
			public string PartnerTimeZone
			{
				get { return (string)this[PartnerTimeZoneProperty]; }
				set { this[PartnerTimeZoneProperty] = value; }
			}

			public static readonly ConfigurationProperty PartnerIdProperty = new ConfigurationProperty("partnerId", typeof(string));
			public string PartnerId
			{
				get { return (string)this[PartnerIdProperty]; }
				set { this[PartnerIdProperty] = value; }
			}

			public static readonly ConfigurationProperty OurIdProperty = new ConfigurationProperty("ourId", typeof(string));
			public string OurId
			{
				get { return (string)this[OurIdProperty]; }
				set { this[OurIdProperty] = value; }
			}

			public static readonly ConfigurationProperty OurClientCodeProperty = new ConfigurationProperty("ourClientCode", typeof(string), "VENTCA01");
			public string OurClientCode
			{
				get { return (string)this[OurClientCodeProperty]; }
				set { this[OurClientCodeProperty] = value; }
			}
		}
	}
}
