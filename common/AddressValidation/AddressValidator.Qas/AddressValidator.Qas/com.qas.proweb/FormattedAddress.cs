/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > FormattedAddress.cs
/// Formatted address details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate data associated with a formatted address
	/// </summary>
	public class FormattedAddress
	{
		// -- Private Members --


		private AddressLine[] m_aAddressLines;
		private bool m_bIsOverflow;
		private bool m_bIsTruncated;
		private DPVStatusType m_dPVStatusField;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="t"></param>
		public FormattedAddress(QAAddressType t)
		{
			m_bIsOverflow = t.Overflow;
			m_bIsTruncated = t.Truncated;
			m_dPVStatusField = t.DPVStatus;

			AddressLineType[] aLines = t.AddressLine;
			// We must have lines in an address so aLines should never be null
			int iSize = aLines.GetLength(0);
			if (iSize > 0)
			{
				m_aAddressLines = new AddressLine[iSize];
				for (int i=0; i < iSize; i++)
				{
					m_aAddressLines[i] = new AddressLine(aLines[i]);
				}
			}
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns the array of address line objects
		/// </summary>
		public AddressLine[] AddressLines
		{
			get
			{
				return m_aAddressLines;
			}
		}

		/// <summary>
		/// Returns the number of lines in the address
		/// </summary>
		public int Length
		{
			get
			{
				return (m_aAddressLines != null ? m_aAddressLines.Length : 0);
			}
		}

		/// <summary>
		/// Flag that indicates there were not enough address lines configured to contain the address
		/// </summary>
		public bool IsOverflow
		{
			get
			{
				return m_bIsOverflow;
			}
		}

		/// <summary>
		/// Flag that indicates one or more address lines were truncated
		/// </summary>
		public bool IsTruncated
		{
			get
			{
				return m_bIsTruncated;
			}
		}

		/// <summary>
		/// DPV Status Type indicates the Delivery point Validation of Address.
		/// </summary>
		public DPVStatusType DPVStatus
		{
			get
			{
				return this.m_dPVStatusField;
			}
			set
			{
				this.m_dPVStatusField = value;
			}
		}		
	}
}
