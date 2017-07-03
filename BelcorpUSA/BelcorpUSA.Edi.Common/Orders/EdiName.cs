using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Common.Orders
{
	/// <summary>
	/// N1 - To identify a party by type of organization, name, and code
	/// </summary>
	public class EdiName
	{
		public string Name { get; set; }

		public string IdentificationQualifier { get; set; }

		public string IdentificationCode { get; set; }
	}

	/// <summary>
	/// N2 - Additional Name Information
	/// </summary>
	public class EdiAdditionalName : EdiName
	{
		public string Address1 { get; set; }
	}

	/// <summary>
	/// N3 - Address Information
	/// </summary>
	public class EdiAddressInformation : EdiAdditionalName
	{
		public string Address2 { get; set; }

		public string Address3 { get; set; }
	}

	/// <summary>
	/// N4 - Geographic Location
	/// </summary>
	public class EdiGeographicLocation : EdiAddressInformation
	{
		public string City { get; set; }

		public string StateProvinceCode { get; set; }

		public string PostalCode { get; set; }

		public string CountryCode { get; set; }
	}
}
