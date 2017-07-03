/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > AddressLine.cs
/// Address line details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// AddressLine encapsulates data associated with an address line of a formatted address.
	/// </summary>
	public class AddressLine
	{
		// -- Public Constants --


		/// <summary>
		/// Enumeration of available line types returned by getLineType()
		/// </summary>
		public enum Types
		{
			None			= LineContentType.None,
			Address		= LineContentType.Address,
			Name			= LineContentType.Name,
			Ancillary	= LineContentType.Ancillary,
			DataPlus		= LineContentType.DataPlus
		}


		// -- Private Members --


		private string m_sLabel;
		private string m_sLine;
		private Types m_eLineType;
		private bool m_bIsTruncated;
		private bool m_bIsOverflow;
        private DataplusGroup[] m_atDataplusGroups;
       

		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="t"></param>
		public AddressLine(AddressLineType t)
		{
			m_sLabel = t.Label;
			m_sLine = t.Line;
			m_eLineType = (Types)t.LineContent;
			m_bIsTruncated = t.Truncated;
			m_bIsOverflow = t.Overflow;

            if ( t.DataplusGroup != null )
            {
                m_atDataplusGroups = new DataplusGroup[t.DataplusGroup.Length];
                
                for ( int i = 0; i < t.DataplusGroup.Length; i++ )
                {
                    DataplusGroup tGroup = new DataplusGroup(t.DataplusGroup[i]);

                    m_atDataplusGroups[i] = tGroup;
                }
            }
		}


        // -- Read-only Properties --


        /// <summary>
        /// Returns the label of the line, probably the name of the address element fixed to it
        /// </summary>
        public string Label
		{
			get
			{
				return m_sLabel;
			}
		}

		/// <summary>
		/// Returns the contents of the address line itself
		/// </summary>
		public string Line
		{
			get
			{
				return m_sLine;
			}
		}

        public DataplusGroup[] DataplusGroups
        {
            get
            {
                return m_atDataplusGroups;
            }
        }

		/// <summary>
		/// Returns the type of the address line (Types enumeration: None ... DataPlus)
		/// </summary>
		public Types LineType
		{
			get
			{
				return m_eLineType;
			}
		}

		/// <summary>
		/// Returns the flag indicating whether the line was truncated
		/// </summary>
		public bool IsTruncated
		{
			get
			{
				return m_bIsTruncated;
			}
		}

		/// <summary>
		/// Returns the flag indicating whether some address elements were lost from this line
		/// </summary>
		public bool IsOverflow
		{
			get
			{
				return m_bIsOverflow;
			}
		}

	}

    /// <summary>
    /// DataplusGroup is a named collection of items attached to an address line
    /// </summary>
    public class DataplusGroup
    {
        // -- Private Members --
        
        private string m_sGroupName;
        private string[] m_asItems;


        /// <summary>
        /// Construct from SOAP-layer object
        /// </summary>
        /// <param name="t"></param>
        public DataplusGroup( DataplusGroupType t )
        {
            m_sGroupName = t.GroupName;
            m_asItems = t.DataplusGroupItem;
        }


        // -- Read-only Properties --

        /// <summary>
        /// Returns the name of this dataplus group ( may be an empty string )
        /// </summary>
        public string Name
        {
            get
            {
                return m_sGroupName;
            }
        }

        /// <summary>
        /// Returns the dataplus items in this group
        /// </summary>
        public string[] Items
        {
            get
            {
                return m_asItems;
            }
        }
    }
}
