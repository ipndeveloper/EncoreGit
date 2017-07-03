/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > PicklistItem.cs
/// Picklist item details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate the data associated with one line of a picklist
	/// </summary>
	public class PicklistItem
	{
		// -- Private Members --


		private string m_sMoniker;
		private string m_sText;
		private string m_sPostcode;
		private int m_iScore;
		private string m_sPartialAddress;

		private bool m_bFullAddress;
		private bool m_bMultiples;
		private bool m_bCanStep;
		private bool m_bAliasMatch;
		private bool m_bPostcodeRecode;
		private bool m_bCrossBorderMatch;
		private bool m_bDummyPOBox;
		private bool m_bName;
		private bool m_bInformation;
		private bool m_bWarnInformation;
		private bool m_bIncompleteAddress;
		private bool m_bUnresolvableRange;
		private bool m_bPhantomPrimaryPoint;
        private bool m_bSubsidiaryData;
        private bool m_bExtendedData;
        private bool m_bEnhancedData;

		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="tItem"></param>
		public PicklistItem(PicklistEntryType tItem)
		{
			m_sText = tItem.Picklist;
			m_sPostcode = tItem.Postcode;
			m_iScore = System.Convert.ToInt32(tItem.Score);
			m_sMoniker = tItem.Moniker;
			m_sPartialAddress = tItem.PartialAddress;

			// Flags
			m_bFullAddress = tItem.FullAddress;
			m_bMultiples = tItem.Multiples;
			m_bCanStep = tItem.CanStep;
			m_bAliasMatch = tItem.AliasMatch;
			m_bPostcodeRecode = tItem.PostcodeRecoded;
			m_bCrossBorderMatch = tItem.CrossBorderMatch;
			m_bDummyPOBox = tItem.DummyPOBox;
			m_bName = tItem.Name;
			m_bInformation = tItem.Information;
			m_bWarnInformation = tItem.WarnInformation;
			m_bIncompleteAddress = tItem.IncompleteAddr;
			m_bUnresolvableRange = tItem.UnresolvableRange;
			m_bPhantomPrimaryPoint = tItem.PhantomPrimaryPoint;
            m_bSubsidiaryData = tItem.SubsidiaryData;
            m_bExtendedData = tItem.ExtendedData;
            m_bEnhancedData = tItem.EnhancedData;
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns the moniker representing this item 
		/// </summary>
		public string Moniker
		{
			get
			{
				return m_sMoniker;
			}
		}

		/// <summary>
		/// Returns the picklist text for display
		/// </summary>
		public string Text
		{
			get
			{
				return m_sText;
			}
		}

		/// <summary>
		/// Returns the postcode for display; may be empty
		/// </summary>
		public string Postcode
		{
			get
			{
				return m_sPostcode;
			}
		}

		/// <summary>
		/// Returns the percentage score of this item; 0 if not applicable
		/// </summary>
		public int Score
		{
			get
			{
				return m_iScore;
			}
		}

		/// <summary>
		/// Returns the score of this item for display, as "100%", or "" if score not applicable
		/// </summary>
		public string ScoreAsString
		{
			get
			{
				if (Score > 0)
				{
					return Score.ToString() + "%";
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Returns the full address details captured thus far 
		/// </summary>
		public string PartialAddress
		{
			get
			{
				return m_sPartialAddress;
			}
		}


		// -- Read-only Property Flags --


		/// <summary>
		/// Indicates whether this item represents a full deliverable address, so can be formatted
		/// </summary>
		public bool IsFullAddress
		{
			get
			{
				return m_bFullAddress;
			}
		}

		/// <summary>
		/// Indicates whether this item represents multiple addresses (for display purposes)
		/// </summary>
		public bool IsMultipleAddresses
		{
			get
			{
				return m_bMultiples;
			}
		}

		/// <summary>
		/// Indicates whether the item can be stepped into
		/// </summary>
		public bool CanStep
		{
			get
			{
				return m_bCanStep;
			}
		}

		/// <summary>
		/// Indicates whether this entry is an alias match, which you may wish to highlight to the user
		/// </summary>
		public bool IsAliasMatch
		{
			get
			{
				return m_bAliasMatch;
			}
		}

		/// <summary>
		/// Indicates whether this entry has a recoded postcode, which you may wish to highlight to the user
		/// </summary>
		public bool IsPostcodeRecoded
		{
			get
			{
				return m_bPostcodeRecode;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a dummy (for DataSets without premise information)
		/// It can neither be stepped into nor formatted, but must be refined against with premise details
		/// </summary>
		public bool IsIncompleteAddress
		{
			get
			{
				return m_bIncompleteAddress;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a range dummy (for DataSets with only ranges of premise information)
		/// It can neither be stepped into nor formatted, but must be refined against with premise details
		/// </summary>
		public bool IsUnresolvableRange
		{
			get
			{
				return m_bUnresolvableRange;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a premise
		/// </summary>
		public bool IsPhantomPrimaryPoint
		{
			get
			{
				return m_bPhantomPrimaryPoint;
			}
		}

		/// <summary>
		/// Indicates whether this entry represents a nearby area, outside the strict initial
		/// boundaries of the search, which you may wish to highlight to the user
		/// </summary>
		public bool IsCrossBorderMatch
		{
			get
			{
				return m_bCrossBorderMatch;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a dummy PO Box (which you may wish to display differently)
		/// </summary>
		public bool IsDummyPOBox
		{
			get
			{
				return m_bDummyPOBox;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a Names item (which you may wish to display differently)
		/// </summary>
		public bool IsName
		{
			get
			{
				return m_bName;
			}
		}

		/// <summary>
		/// Indicates whether this entry is an informational prompt, rather than an address
		/// </summary>
		public bool IsInformation
		{
			get
			{
				return m_bInformation;
			}
		}

		/// <summary>
		/// Indicates whether this entry is a warning prompt, indicating that it is not possible to
		/// proceed any further (due to no matches, too many matches, etc.)
		/// </summary>
		public bool IsWarnInformation
		{
			get
			{
				return m_bWarnInformation;
			}
		}

        /// <summary>
        /// Indicates that this entry is from the subsidiary rather than the base data set
        /// </summary>
        public bool IsSubsidiaryData
        {
            get
            {
                return m_bSubsidiaryData;
            }
        }

        /// <summary>
        /// Indicates that this entry is from the base data set but extended by the subsidiary data set
        /// </summary>
        public bool IsExtendedData
        {
            get
            {
                return m_bExtendedData;
            }
        }

        /// <summary>
        /// Indicates that this entry is from the base data set but enhanced by the subsidiary data set
        /// </summary>
        public bool IsEnhancedData
        {
            get
            {
                return m_bEnhancedData;
            }
        }
	}
}
