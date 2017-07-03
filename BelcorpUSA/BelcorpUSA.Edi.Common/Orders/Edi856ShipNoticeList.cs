using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using System.IO;
using NetSteps.Diagnostics.Utilities;

namespace BelcorpUSA.Edi.Common.Orders
{
	public class Edi856ShipNoticeList : EdiX12List<Edi856ShipNotice>
	{
		private string _filename;
		/// <summary>
		/// File name that this record was read from or stored to.
		/// </summary>
		public override string FileName
		{
			get { return _filename; }
			set
			{
				if (_filename != value)
				{
					_filename = value;
					if (this.Count > 0)
					{
						this.ForEach(i => i.FileName = _filename);
					}
				}
			}
		}

		public static Edi856ShipNoticeList FromFile(string edi856FileName, string timeZoneId)
		{
			var list = ParseEdi856(File.ReadAllText(edi856FileName), timeZoneId);
			list.FileName = edi856FileName;
			return list;
		}

		public static Edi856ShipNoticeList ParseEdi856(string edi856, string timeZoneId)
		{
			TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			var result = new Edi856ShipNoticeList();

			var parser = new X12Parser();
			IEnumerable<Interchange> interchangeList = parser.ParseMultiple(edi856);

			foreach (var interchange in interchangeList)
			{
				try
				{
					if (result.InterchangeControlNumber == 0)
					{
						result.InterchangeControlNumber = Int32.Parse(interchange.InterchangeControlNumber);
						result.InterchangeDateUTC = TimeZoneInfo.ConvertTimeToUtc(interchange.InterchangeDate, tz);
						result.SenderId = interchange.InterchangeSenderId.Trim();
						result.RecieverId = interchange.InterchangeReceiverId.Trim();
					}
					var fg = interchange.FunctionGroups.First();
					foreach (var trans in fg.Transactions)
					{
						var ship = new Edi856ShipNotice();
						/// BSN = Begin Ship Notice
						var bsn = trans.Segments.First(s => s.SegmentId == "BSN");
						int shipId;
						if (Int32.TryParse(bsn.GetElement(2), out shipId))
						{
							ship.ShipmentId = shipId;
						}
						else
						{
							typeof(Edi856ShipNoticeList).TraceError(String.Format("Recieved EDI856 with BSN ID of {0}, expected integer value", bsn.GetElement(2)));
						}

						var hl = trans.HLoops.First();

						///TD1 = Carrier Details (Quantity and Weight)
						var td1 = hl.Segments.First(s => s.SegmentId == "TD1");
						ship.ShipmentDetails.PackingCode = td1.GetElement(1);
						ship.ShipmentDetails.LadingQuantity = Int32.Parse(td1.GetElement(2));
						ship.ShipmentDetails.WeightQualifier = td1.GetElement(6);
						ship.ShipmentDetails.Weight = Decimal.Parse(td1.GetElement(7));
						ship.ShipmentDetails.WeightUnitOfMeasure = td1.GetElement(8);

						///TD5 = Carrier Details (Routing Sequence/Transit Time)
						var td5 = hl.Segments.First(s => s.SegmentId == "TD5");
						ship.ShipmentDetails.RoutingSequenceCode = td5.GetElement(1);
						ship.ShipmentDetails.IdentificationCode = td5.GetElement(3);
						ship.ShipmentDetails.TransportationMethod = td5.GetElement(4);
						ship.ShipmentDetails.Routing = td5.GetElement(5);

						foreach (var refSeg in hl.Segments.Where(s => s.SegmentId == "REF"))
						{
							switch (refSeg.GetElement(1))
							{
								case "CN":
									ship.ShipmentDetails.TrackingNumbers.Add(refSeg.GetElement(2));
									break;
								default:
									ship.References.Add(new EdiReference()
									{
										IdentificationQualifier = refSeg.GetElement(1),
										Identification = refSeg.GetElement(2)
									});
									break;
							}
						}

						///DTM = Date Time Reference, Element 1 = Qualifier, Element 1 Value '011' = Shipped
						var dtm = hl.Segments.First(s => s.SegmentId == "DTM" && s.GetElement(1) == "011");
						DateTime sdate = dtm.GetDate8Element(2).GetValueOrDefault();
						if (sdate != DateTime.MinValue)
						{
							sdate = TimeZoneInfo.ConvertTimeToUtc(sdate, tz);
						}
						ship.ShippedDateUTC = sdate;

						///N1 Loop = Name, Element 1 = Qualifier, Element 1 Value 'SF' = Ship From
						var sf = hl.Loops.First(l => l.SegmentId == "N1" && l.GetElement(1) == "SF");
						ship.ShipFrom.Name = sf.GetElement(2).Trim();
						ship.ShipFrom.IdentificationQualifier = sf.GetElement(3);
						ship.ShipFrom.IdentificationCode = sf.GetElement(4);

						///N1 Loop = Name, Element 1 = Qualifier, Element 1 Value 'ST' = Ship To
						var st = hl.Loops.First(l => l.SegmentId == "N1" && l.GetElement(1) == "ST");
						ship.ShipTo.Name = st.GetElement(2).Trim();
						ship.ShipTo.IdentificationQualifier = st.GetElement(3);
						ship.ShipTo.IdentificationCode = st.GetElement(4);
						///N4 Sub Loop Geographical Location
						var stGeo = st.Segments.First(s => s.SegmentId == "N4");
						ship.ShipTo.City = stGeo.GetElement(1);
						ship.ShipTo.StateProvinceCode = stGeo.GetElement(2);
						ship.ShipTo.PostalCode = stGeo.GetElement(3);

						///Step down the Hierarchical loop structure to Item level
						foreach (var olev in hl.HLoops)
						{
							//PRF - Purchase Order Reference
							var prf = hl.Segments.FirstOrDefault(s => s.SegmentId == "PRF");
							if (prf != null)
							{
								int soid;
								if (Int32.TryParse(prf.GetElement(1), out soid))
								{
									ship.OrderId = soid;
								}
							}
							foreach (var plev in olev.HLoops)
							{
								var container = new Edi856ShipNotice.EdiShipmentContainer();
								foreach (var ilev in plev.HLoops)
								{
									///LIN = Item Identification
									var lin = ilev.Segments.First(s => s.SegmentId == "LIN");
									///SN1 = Item Detail (Shipment)
									var sn1 = ilev.Segments.First(s => s.SegmentId == "SN1");
									container.Items.Add(new Edi856ShipNotice.EdiShipmentContainer.EdiShipmentItem()
									{
										LineNumber = lin.GetIntElement(1).GetValueOrDefault(),
										ProductCode = lin.GetElement(3),
										LotCode = lin.GetElement(5),
										QuantityShipped = sn1.GetIntElement(2).GetValueOrDefault(),
										UnitOfMeasureShipped = sn1.GetElement(3),
										QuantityOrdered = sn1.GetIntElement(5).GetValueOrDefault(),
										UnitOfMeasureOrdered = sn1.GetElement(6)
									});
								}
								ship.Containers.Add(container);
							}
						}
						result.Add(ship);
					}
				}
				catch (Exception ex)
				{
					typeof(Edi856ShipNoticeList).TraceException(ex);
					throw;
				}
			}

			return result;
		}
	}

	public class Edi856ShipNotice
	{
		public string FileName { get; set; }

		public int OrderId { get; set; }

		public int ShipmentId { get; set; }

		public EdiName ShipFrom { get; set; }

		public EdiGeographicLocation ShipTo { get; set; }

		public ICollection<EdiReference> References { get; set; }

		public EdiCarrierDetails ShipmentDetails { get; set; }

		public DateTime ShippedDateUTC { get; set; }

		public ICollection<EdiShipmentContainer> Containers { get; set; }

		public Edi856ShipNotice()
		{
			this.ShipFrom = new EdiName();
			this.ShipTo = new EdiGeographicLocation();
			this.References = new List<EdiReference>();
			this.ShipmentDetails = new EdiCarrierDetails();
			this.Containers = new List<EdiShipmentContainer>();
		}

		public class EdiCarrierDetails
		{
			public List<string> TrackingNumbers { get; private set; }
			public string PackingCode { get; set; }
			public int LadingQuantity { get; set; }
			public string WeightQualifier { get; set; }
			public decimal Weight { get; set; }
			public string WeightUnitOfMeasure { get; set; }
			public string RoutingSequenceCode { get; set; }
			public string IdentificationCode { get; set; }
			public string TransportationMethod { get; set; }
			public string Routing { get; set; }

			public EdiCarrierDetails()
			{
				this.TrackingNumbers = new List<string>();
			}
		}

		public class EdiShipmentContainer
		{
			public ICollection<EdiShipmentItem> Items { get; set; }

			public EdiShipmentContainer()
			{
				this.Items = new List<EdiShipmentItem>();
			}

			public class EdiShipmentItem
			{
				public int LineNumber { get; set; }
				public string ProductCode { get; set; }
				public string LotCode { get; set; }
				public int QuantityShipped { get; set; }
				public string UnitOfMeasureShipped { get; set; }
				public int QuantityOrdered { get; set; }
				public string UnitOfMeasureOrdered { get; set; }
			}
		}
	}
}
