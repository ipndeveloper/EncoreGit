/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > LicensedSet.cs
/// Data licencing details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulates data for a single licensed set
	/// </summary>
	public class LicensedSet
	{
		// -- Public Constants --


		/// <summary>
		/// Enumeration of warning levels that can be returned
		/// </summary>
		public enum WarningLevels
		{
			None				    = LicenceWarningLevel.None,
			DataExpiring			= LicenceWarningLevel.DataExpiring,
			LicenceExpiring			= LicenceWarningLevel.LicenceExpiring,
			ClicksLow			= LicenceWarningLevel.ClicksLow,
			Evaluation			= LicenceWarningLevel.Evaluation,
			NoClicks			= LicenceWarningLevel.NoClicks,
			DataExpired			= LicenceWarningLevel.DataExpired,
			EvalLicenceExpired	= LicenceWarningLevel.EvalLicenceExpired,
			FullLicenceExpired	= LicenceWarningLevel.FullLicenceExpired,
			LicenceNotFound		= LicenceWarningLevel.LicenceNotFound,
			DataUnreadable		= LicenceWarningLevel.DataUnreadable
		}


		// -- Private Members --


		private string m_sID;
		private string m_sDescription;
		private string m_sCopyright;
		private string m_sVersion;
		private string m_sBaseCountry;
		private string m_sStatus;
		private string m_sServer;
		private WarningLevels m_eWarningLevel;
		private int m_iDaysLeft;			// non-negative
		private int m_iDataDaysLeft;		// non-negative
		private int m_iLicenceDaysLeft;	// non-negative


		// -- Public Methods --

		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="s"></param>
		public LicensedSet(QALicensedSet s)
		{
			m_sID = s.ID;
			m_sDescription = s.Description;
			m_sCopyright = s.Copyright;
			m_sVersion = s.Version;
			m_sBaseCountry = s.BaseCountry;
			m_sStatus = s.Status;
			m_sServer = s.Server;
			m_eWarningLevel = (WarningLevels)s.WarningLevel;
			m_iDaysLeft = System.Convert.ToInt32(s.DaysLeft);
			m_iDataDaysLeft = System.Convert.ToInt32(s.DataDaysLeft);
			m_iLicenceDaysLeft = System.Convert.ToInt32(s.LicenceDaysLeft);
		}

		/// <summary>
		/// Create array of LicensedSets given a SOAP-layer QALicenceInfo object.
		/// We do not directly represent the QALicenceInfo structure, so lose it's warningLevel member
		/// We simply return an array of LicensedSets.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static LicensedSet[] createArray(QALicenceInfo info)
		{
			LicensedSet[] aResults = null;
			QALicensedSet[] aInfo = info.LicensedSet;
			if (aInfo != null)
			{
				int iSize = aInfo.GetLength(0);
				if (iSize > 0)
				{
					aResults = new LicensedSet[iSize];
					for (int i=0; i < iSize; i++)
					{
						aResults[i] = new LicensedSet(aInfo[i]);
					}
				}
			}
			return aResults;
		}

        public static LicensedSet[] createArray(QADataMapDetail info)
        {
            LicensedSet[] aResults = null;
            QALicensedSet[] aInfo = info.LicensedSet;

            if ( aInfo != null )
            {
                if ( aInfo.Length > 0 )
                {
                    aResults = new LicensedSet[aInfo.Length];

                    for ( int i = 0; i < aInfo.Length; ++i )
                    {
                        aResults[i] = new LicensedSet(aInfo[i]);
                    }
                }
            }

            return aResults;
        }


		// -- Read-only Properties --


		/// <summary>
		/// Returns the ID of the licensed data
		/// </summary>
		public string ID
		{
			get
			{
				return m_sID;
			}
		}

		/// <summary>
		/// Returns the description of the licensed data
		/// </summary>
		public string Description
		{
			get
			{
				return m_sDescription;
			}
		}

		/// <summary>
		/// Returns the owner of the copyright for the licensed data
		/// </summary>
		public string Copyright
		{
			get
			{
				return m_sCopyright;
			}
		}

		/// <summary>
		/// Returns the version of the licensed data
		/// </summary>
		public string Version
		{
			get
			{
				return m_sVersion;
			}
		}

		/// <summary>
		/// Returns the DataId of the country with which this licensed dataset is associated
		/// </summary>
		public string BaseCountry
		{
			get
			{
				return m_sBaseCountry;
			}
		}

		/// <summary>
		/// Returns status text detailing the amount of time left on the licensed set
		/// </summary>
		public string Status
		{
			get
			{
				return m_sStatus;
			}
		}

		/// <summary>
		/// Returns the name of the QAS server where the data is being used
		/// </summary>
		public string Server
		{
			get
			{
				return m_sServer;
			}
		}

		/// <summary>
		/// Returns the enumeration of the state of the data set
		/// </summary>
		public WarningLevels WarningLevel
		{
			get
			{
				return m_eWarningLevel;
			}
		}

		/// <summary>
		/// Returns the number of days that the data will remain usable
		/// </summary>
		public int DaysLeft
		{
			get
			{
				return m_iDaysLeft;
			}
		}

		/// <summary>
		/// Returns the number of days until the data expires
		/// </summary>
		public int DataDaysLeft
		{
			get
			{
				return m_iDataDaysLeft;
			}
		}

		/// <summary>
		/// Returns the number of days until the license expires for this data
		/// </summary>
		public int LicenceDaysLeft
		{
			get
			{
				return m_iLicenceDaysLeft;
			}
		}
	}
}
