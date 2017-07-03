/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > SearchResult.cs
/// The results of a search


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Class to encapsulate data returned by a search
	/// </summary>
	public class SearchResult
	{
		// -- Public Constants --


		/// <summary>
		/// Enumeration of verification levels
		/// </summary>
		public enum VerificationLevels
		{
			// No verified matches found, or not application
			None = VerifyLevelType.None,
			// High confidence match found (address returned)
			Verified = VerifyLevelType.Verified,
			// Single match found, but user confirmation is recommended (address returned)
			InteractionRequired = VerifyLevelType.InteractionRequired,
			// Address was verified to premises level only (picklist returned)
			PremisesPartial = VerifyLevelType.PremisesPartial,
			// Address was verified to street level only (picklist returned)
			StreetPartial = VerifyLevelType.StreetPartial,
			// Address was verified to multiple addresses (picklist returned)
			Multiple = VerifyLevelType.Multiple,
			// Address was verified to place level (global data only)
			VerifiedPlace = VerifyLevelType.VerifiedPlace,
			// Address was verified to street level (global data only)
			VerifiedStreet = VerifyLevelType.VerifiedStreet
		}


		// -- Private Members --


		private FormattedAddress m_Address = null;
		private Picklist m_Picklist = null;
		private VerificationLevels m_eVerifyLevel = VerificationLevels.None;


		// -- Public Methods --


		/// <summary>
		/// Construct from a SOAP-layer object 
		/// </summary>
		/// <param name="sr"></param>
		public SearchResult(QASearchResult sr)
		{
			QAAddressType address = sr.QAAddress;
			if (address != null)
			{
				m_Address = new FormattedAddress(address);
			}

			QAPicklistType picklist = sr.QAPicklist;
			if (picklist != null)
			{
				m_Picklist = new Picklist(picklist);
			}

			m_eVerifyLevel = (VerificationLevels)sr.VerifyLevel;
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns the address (may be null)
		/// </summary>
		public FormattedAddress Address
		{
			get
			{
				return m_Address;
			}
		}

		/// <summary>
		/// Returns the picklist (may be null)
		/// </summary>
		/// <returns></returns>
		public Picklist Picklist
		{
			get
			{
				return m_Picklist;
			}
		}

		/// <summary>
		/// Returns the verification level of the result (only relavant when using the verification engine)
		/// </summary>
		public VerificationLevels VerifyLevel
		{
			get
			{
				return m_eVerifyLevel;
			}
		}
	}
}
