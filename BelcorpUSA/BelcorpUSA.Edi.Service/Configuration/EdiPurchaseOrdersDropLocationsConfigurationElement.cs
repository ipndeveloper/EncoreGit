using System.Configuration;
using NetSteps.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiPurchaseOrdersDropLocationsConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty PaidProperty = new ConfigurationProperty("paid", typeof(EdiDropLocationCollectionElement));
		public EdiDropLocationCollectionElement Paid
		{
			get { return (EdiDropLocationCollectionElement)this[PaidProperty]; }
			set { this[PaidProperty] = value; }
		}

		public static readonly ConfigurationProperty CanceledProperty = new ConfigurationProperty("canceled", typeof(EdiDropLocationCollectionElement));
		public EdiDropLocationCollectionElement Canceled
		{
			get { return (EdiDropLocationCollectionElement)this[CanceledProperty]; }
			set { this[CanceledProperty] = value; }
		}

		public static readonly ConfigurationProperty ReturnedProperty = new ConfigurationProperty("returned", typeof(EdiDropLocationCollectionElement));
		public EdiDropLocationCollectionElement Returned
		{
			get { return (EdiDropLocationCollectionElement)this[ReturnedProperty]; }
			set { this[ReturnedProperty] = value; }
		}
	}

	public class EdiDropLocationCollectionElement : GenericConfigurationElementCollection<EdiDropLocationConfigurationElement>
	{
		public EdiDropLocationCollectionElement()
			: base((i) => i.PartnerName) { }
	}
}
