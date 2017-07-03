/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > CanSearch.cs
/// Details about searching availability


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate the result of a CanSearch operation:
	/// searching availability, and the reasons when unavailable
	/// </summary>
	public class CanSearch
	{
		// -- Private Members --


		private bool m_bOk;
		private string m_sErrorMessage;
		private int m_iError;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		public CanSearch(QASearchOk tResult)
		{
			m_bOk = tResult.IsOk;

			if (tResult.ErrorCode != null)
			{
				m_iError = System.Convert.ToInt32(tResult.ErrorCode);
			}
			if (tResult.ErrorMessage != null)
			{
				m_sErrorMessage = tResult.ErrorMessage + " [" + m_iError + "]";
			}
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns whether searching is possible for the requested data-engine-layout combination
		/// </summary>
		public bool IsOk
		{
			get
			{
				return m_bOk;
			}
		}

		/// <summary>
		/// Returns error information relating why it is not possible to search the requested data-engine-layout
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return m_sErrorMessage;
			}
		}
	}
}
