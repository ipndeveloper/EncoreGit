using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OopFactory.X12;
using System.IO;
using OopFactory.X12.Parsing.Model;
using System.Diagnostics.Contracts;

namespace BelcorpUSA.Edi.Common.Orders
{
	public class Edi850PurchaseOrderList : EdiX12List<Edi850PurchaseOrder>
	{
		public string ClientCode { get; set; }

		public string ConvertOrderToX12850Document(bool isProduction, string timeZone, char segmentTerminator = '\n', char elementSeperator = '*', char subelementSeperator = '~')
		{
			TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			Interchange interchange = new Interchange(TimeZoneInfo.ConvertTimeFromUtc(this.InterchangeDateUTC, tz), this.InterchangeControlNumber, isProduction, '~', elementSeperator, subelementSeperator);
			interchange.InterchangeSenderIdQualifier = this.SenderIdQualifier;
			interchange.InterchangeSenderId = this.SenderId;
			interchange.InterchangeReceiverIdQualifier = this.RecieverIdQualifier;
			interchange.InterchangeReceiverId = this.RecieverId;
			interchange.SetElement(14, "1");//No Acknowledgement

			/// Arg3 is the Control Number.  This is a 1 to 9 numeric value.  This is a number that needs to be unique inside the ISA envelope only.  
			/// Some implementations use an incrementing number that increments with the usage like the ISA control number.
			/// Others just have a count of the number of GS envelopes inside an ISA.
			/// Both are acceptable.
			/// All that is required is that the GS control number be unique among other GS control numbers within the ISA envelope that it is contained in.
			/// In this case there is only one function group, so set it's Control Number to 1
			var group = interchange.AddFunctionGroup("PO", TimeZoneInfo.ConvertTimeFromUtc(this.InterchangeDateUTC, tz), 1, "004010");

			group.ApplicationReceiversCode = this.RecieverId;
			group.ApplicationSendersCode = this.SenderId;

			int i = 0;
			foreach (var po in this)
			{
				var transaction = group.AddTransaction("850", (++i).ToString().PadLeft(9, '0'));
				var begSegment = transaction.AddSegment("BEG");
				begSegment.SetElement(1, "00");
				begSegment.SetElement(2, "SA");
				begSegment.SetElement(3, po.ShipmentId);
				begSegment.SetElement(5, TimeZoneInfo.ConvertTimeFromUtc(po.OrderDateUTC, tz).ToString("yyyyMMdd"));

				if (!String.IsNullOrWhiteSpace(this.ClientCode))
				{
					var refCrSegment = transaction.AddSegment("REF");
					refCrSegment.SetElement(1, "CR");
					refCrSegment.SetElement(2, this.ClientCode);
				}

				var refPoSegment = transaction.AddSegment("REF");
				refPoSegment.SetElement(1, "PO");
				refPoSegment.SetElement(2, po.OrderId);

				var refTnSegment = transaction.AddSegment("REF");
				refTnSegment.SetElement(1, "TN");
				refTnSegment.SetElement(2, po.AccountId);

				var refTypeSegment = transaction.AddSegment("REF");
				refTypeSegment.SetElement(1, "PH");
				refTypeSegment.SetElement(2, po.StatusCode);

				var fobSegment = transaction.AddSegment("FOB");
				fobSegment.SetElement(1, "PP");


				var dtmDate = po.OrderDateUTC;
				if (DateTime.UtcNow.Date > dtmDate.Date)
				{
					dtmDate = DateTime.UtcNow;
				}

				var dtmSegment = transaction.AddSegment("DTM");
				dtmSegment.SetElement(1, "037");
				dtmSegment.SetElement(2, TimeZoneInfo.ConvertTimeFromUtc(dtmDate, tz).ToString("yyyyMMdd"));

				if (!String.IsNullOrWhiteSpace(po.ShipBy))
				{
					var td5 = transaction.AddSegment("TD5");
					td5.SetElement(1, "O");
					td5.SetElement(2, "2");
					td5.SetElement(3, po.ShipBy);
				}

				var shipFromLoop = transaction.AddLoop("N1");
				shipFromLoop.SetElement(1, "SF");
				shipFromLoop.SetElement(2, po.ShipFrom.Name);
				shipFromLoop.SetElement(3, po.ShipFrom.IdentificationQualifier);
				shipFromLoop.SetElement(4, po.ShipFrom.IdentificationCode);

				var shiptoLoop = transaction.AddLoop("N1");
				shiptoLoop.SetElement(1, "ST");
				shiptoLoop.SetElement(2, po.ShipTo.Name);

				var shiptoaddr1 = shiptoLoop.AddSegment("N2");
				shiptoaddr1.SetElement(1, po.ShipTo.Address1);

				if (!String.IsNullOrWhiteSpace(po.ShipTo.Address2))
				{
					var shiptoaddr2 = shiptoLoop.AddSegment("N3");
					shiptoaddr2.SetElement(1, po.ShipTo.Address2);
					if (!String.IsNullOrWhiteSpace(po.ShipTo.Address3))
					{
						shiptoaddr2.SetElement(2, po.ShipTo.Address3);
					}
				}

				var shipToCsp = shiptoLoop.AddSegment("N4");
				shipToCsp.SetElement(1, po.ShipTo.City);
				shipToCsp.SetElement(2, po.ShipTo.StateProvinceCode);
				shipToCsp.SetElement(3, po.ShipTo.PostalCode);
				shipToCsp.SetElement(4, po.ShipTo.CountryCode);

				int itemCount = 1;
				foreach (var poItem in po.Items)
				{
					var lineItem = transaction.AddLoop("PO1");
					lineItem.SetElement(1, itemCount++);
					lineItem.SetElement(2, poItem.Quantity);
					lineItem.SetElement(3, poItem.UnitOfMeasure);
					lineItem.SetElement(6, poItem.ProductCodeQualifier);
					lineItem.SetElement(7, poItem.ProductCode);
					lineItem.SetElement(12, poItem.CatalogCodeQualifier);
					lineItem.SetElement(13, poItem.CatalogCode);

					var ref07 = transaction.AddSegment("REF");
					ref07.SetElement(1, "07");
					ref07.SetElement(2, poItem.CommercialMovementCode);
				}

				transaction.AddLoop("CTT").SetElement(1, po.Items.Count());
			}

			return interchange.SerializeToX12(false).Replace('~', segmentTerminator); //This Lib doesn't support '\n' as a segment terminator, uisng default '~' and replacing with default provided
		}
	}

	public class Edi850PurchaseOrder
	{
		#region Properties

		public int AccountId { get; set; }

		public int OrderId { get; set; }

		public int ShipmentId { get; set; }

		public string StatusCode { get; set; }

		public DateTime OrderDateUTC { get; set; }

		public EdiName ShipFrom { get; set; }

		public EdiGeographicLocation ShipTo { get; set; }

		public string ShipBy { get; set; }

		public IEnumerable<Edi850POItem> Items { get; set; }

		#endregion

		#region Constructors

		public Edi850PurchaseOrder()
		{
			this.ShipTo = new EdiGeographicLocation();
			this.ShipFrom = new EdiName();
			this.Items = new List<Edi850POItem>();
		}

		#endregion

		/// <summary>
		/// PO1 - To specify basic and most frequently used line item data
		/// </summary>
		public class Edi850POItem
		{
			/// <summary>
			/// PO102 - Quantity Ordered
			/// </summary>
			public int Quantity { get; set; }

			/// <summary>
			/// PO103 - Code specifying the units in which a value is being expressed, or manner in which a measurement has been taken
			/// <example>EA<remarks> - Represents 'Each'</remarks></example>
			/// </summary>
			public string UnitOfMeasure { get; set; }

			/// <summary>
			/// PO106 - Code identifying the type/source of the descriptive number used in Product/Service ID
			/// <example>VN<remarks> - Represents 'Vendor Number'</remarks></example>
			/// </summary>
			public string ProductCodeQualifier { get; set; }

			/// <summary>
			/// PO107 - Product or Service Id - Identifying number for a product or service
			/// </summary>
			public string ProductCode { get; set; }

			/// <summary>
			/// PO112 - Product/Service ID Qualifier
			/// <example>CB<remarks> - Represents 'Buyers Catalog Number'</remarks></example>
			/// </summary>
			public string CatalogCodeQualifier { get; set; }

			/// <summary>
			/// PO113
			/// </summary>
			public string CatalogCode { get; set; }

			/// <summary>
			/// REF 07 2 - Belcorp requires an additional "Commercial Movement Type" code added with each item record...
			/// </summary>
			public string CommercialMovementCode { get; set; }
		}
	}
}
