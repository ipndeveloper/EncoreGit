/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > Picklist.cs
/// Picklist details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate Picklist data
	/// </summary>
	public class Picklist
	{
		// -- Private Members --


		private string m_sMoniker;
		private PicklistItem[] m_aItems;
		private string m_sPrompt;
		private int m_iTotal;

		private bool m_bAutoStepinSafe;
		private bool m_bAutoStepinPastClose;
		private bool m_bAutoFormatSafe;
		private bool m_bAutoFormatPastClose;
		private bool m_bLargePotential;
		private bool m_bMaxMatches;
		private bool m_bMoreOtherMatches;
		private bool m_bOverThreshold;
		private bool m_bTimeout;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="p"></param>
		public Picklist(QAPicklistType p)
		{
			m_iTotal = System.Convert.ToInt32(p.Total);
			m_sMoniker = p.FullPicklistMoniker;
			m_sPrompt = p.Prompt;
			m_bAutoStepinSafe = p.AutoStepinSafe;
			m_bAutoStepinPastClose = p.AutoStepinPastClose;
			m_bAutoFormatSafe = p.AutoFormatSafe;
			m_bAutoFormatPastClose = p.AutoFormatPastClose;
			m_bLargePotential = p.LargePotential;
			m_bMaxMatches = p.MaxMatches;
			m_bMoreOtherMatches = p.MoreOtherMatches;
			m_bOverThreshold = p.OverThreshold;
			m_bTimeout = p.Timeout;
			
			// Convert the lines in the picklist
			m_aItems = null;
			PicklistEntryType[] aItems = p.PicklistEntry;
			// Check for null as we can have an empty picklist
			if (aItems != null)
			{
				int iSize = aItems.GetLength(0);
				if (iSize > 0)
				{
					m_aItems = new PicklistItem[iSize];
					for (int i=0; i < iSize; i++)
					{
						m_aItems[i] = new PicklistItem(aItems[i]);
					}
				}
			}
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns the full picklist moniker; that is, the moniker that describes this entire picklist
		/// </summary>
		public string Moniker
		{
			get
			{
				return m_sMoniker;
			}
		}

		/// <summary>
		/// Returns the array of PicklistItem objects
		/// </summary>
		public PicklistItem[] Items
		{
			get
			{
				return m_aItems;
			}
		}

		/// <summary>
		/// Returns the number of items in the picklist
		/// </summary>
		public int Length
		{
			get
			{
				return (m_aItems != null ? m_aItems.Length : 0);
			}
		}

		/// <summary>
		/// Returns the prompt indicating what should be entered next by the user
		/// </summary>
		public string Prompt
		{
			get
			{
				return m_sPrompt;
			}
		}

		/// <summary>
		/// Returns the total number of addresses (excluding informationals) within this address location (approx)
		/// </summary>
		public int Total
		{
			get
			{
				return m_iTotal;
			}
		}


		// -- Read-only Property Flags --


		/// <summary>
		/// Indicates that it is safe to automatically step-in to the first (and only) picklist item
		/// </summary>
		public bool IsAutoStepinSafe
		{
			get
			{
				return m_bAutoStepinSafe;
			}
		}

		/// <summary>
		/// Indicates that you may wish to automaticaly step-in to the first item, as 
		/// there was only one exact match, and other close matches
		/// </summary>
		public bool IsAutoStepinPastClose
		{
			get
			{
				return m_bAutoStepinPastClose;
			}
		}

		/// <summary>
		/// Indicates whether the picklist contains a single non-informational step-in item
		/// which you may wish to automatically step into after a refinement
		/// </summary>
		public bool IsAutoStepinSingle
		{
			get
			{
				return Length == 1
					&& Items[0].CanStep
					&& !Items[0].IsInformation;
			}
		}

		/// <summary>
		/// Indicates that it is safe to automatically format the first (and only) picklist item
		/// </summary>
		public bool IsAutoFormatSafe
		{
			get
			{
				return m_bAutoFormatSafe;
			}
		}

		/// <summary>
		/// Indicates that you may wish to automatically format the first item, as
		/// there was only one exact match, and other close matches
		/// </summary>
		public bool IsAutoFormatPastClose
		{
			get
			{
				return m_bAutoFormatPastClose;
			}
		}

		/// <summary>
		/// Indicates that the picklist contains a single non-informational final-address item
		/// which you may wish to automatically format after a refinement
		/// </summary>
		public bool IsAutoFormatSingle
		{
			get
			{
				return Length == 1
					&& Items[0].IsFullAddress
					&& !Items[0].IsInformation;
			}
		}

		/// <summary>
		/// Indicates that the picklist potentially contains too many items to display
		/// </summary>
		public bool IsLargePotential
		{
			get
			{
				return m_bLargePotential;
			}
		}

		/// <summary>
		/// Indicates that the number of matches exceeded the maximum allowed
		/// </summary>
		public bool IsMaxMatches
		{
			get
			{
				return m_bMaxMatches;
			}
		}

		/// <summary>
		/// Indicates that there are additional matches that can be displayed
		/// Only exact matches to the refinement text have been shown, as including all matches would be over threshold
		/// They can be shown by stepping into the informational at the bottom of the picklist
		/// </summary>
		public bool AreMoreMatches
		{
			get
			{
				return m_bMoreOtherMatches;
			}
		}

		/// <summary>
		/// Indicates that the number of matches exceeded the threshold
		/// </summary>
		public bool IsOverThreshold
		{
			get
			{
				return m_bOverThreshold;
			}
		}

		/// <summary>
		/// Indicates that the search timed out
		/// </summary>
		public bool IsTimeout
		{
			get
			{
				return m_bTimeout;
			}
		}
	}
}
