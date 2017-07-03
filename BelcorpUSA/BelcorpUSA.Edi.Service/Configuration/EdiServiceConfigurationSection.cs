using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Configuration;
using System.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiServiceConfigurationSection : DeclaredMemberConfigurationSection
	{
		public static readonly ConfigurationProperty IsProductionProperty = new ConfigurationProperty("isProduction", typeof(bool), false);
		public bool IsProduction
		{
			get { return (bool)this[IsProductionProperty]; }
			set { this[IsProductionProperty] = value; }
		}

		public static readonly ConfigurationProperty EdiInterchangeProperty = new ConfigurationProperty("interchange", typeof(EdiInterchangeConfigurationElement));
		public EdiInterchangeConfigurationElement EdiInterchange
		{
			get { return (EdiInterchangeConfigurationElement)this[EdiInterchangeProperty]; }
			set { this[EdiInterchangeProperty] = value; }
		}

		public static readonly ConfigurationProperty ShipNoticeDropLocationsProperty = new ConfigurationProperty("shipNoticeDropLocations", typeof(EdiDropLocationCollectionElement));
		public EdiDropLocationCollectionElement ShipNoticeDropLocations
		{
			get { return (EdiDropLocationCollectionElement)this[ShipNoticeDropLocationsProperty]; }
			set { this[ShipNoticeDropLocationsProperty] = value; }
		}

		public static readonly ConfigurationProperty PurchaseOrdersDropLocationsProperty = new ConfigurationProperty("purchaseOrdersDropLocations", typeof(EdiPurchaseOrdersDropLocationsConfigurationElement));
		public EdiPurchaseOrdersDropLocationsConfigurationElement PurchaseOrdersDropLocations
		{
			get { return (EdiPurchaseOrdersDropLocationsConfigurationElement)this[PurchaseOrdersDropLocationsProperty]; }
			set { this[PurchaseOrdersDropLocationsProperty] = value; }
		}

		public static readonly ConfigurationProperty ArchivalProperty = new ConfigurationProperty("archival", typeof(EdiArchivalConfigurationElement));
		public EdiArchivalConfigurationElement Archival
		{
			get { return (EdiArchivalConfigurationElement)this[ArchivalProperty]; }
			set { this[ArchivalProperty] = value; }
		}

		public static readonly ConfigurationProperty WorkingFolderProperty = new ConfigurationProperty("workingFolder", typeof(EdiWorkingFolderConfigurationElement));
		public EdiWorkingFolderConfigurationElement WorkingFolder
		{
			get { return (EdiWorkingFolderConfigurationElement)this[WorkingFolderProperty]; }
			set { this[WorkingFolderProperty] = value; }
		}
	}
}
