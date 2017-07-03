/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > ExampleAddress.cs
/// Example address details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate example address data
	/// </summary>
	public class ExampleAddress
	{
		// -- Private Members --


      private string m_sComment;
      private FormattedAddress m_Address;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="a"></param>
		public ExampleAddress(QAExampleAddress a)
		{
			m_sComment = a.Comment;
			m_Address = new FormattedAddress(a.Address);
		}

		/// <summary>
		/// Create array from SOAP-layer array
		/// </summary>
		/// <param name="aAddresses"></param>
		/// <returns></returns>
		public static ExampleAddress[] createArray(QAExampleAddress[] aAddresses)
		{
			ExampleAddress[] aResults = null;
			if (aAddresses != null)
			{
				int iSize = aAddresses.GetLength(0);
				if (iSize > 0)
				{
					aResults = new ExampleAddress[iSize];
					for (int i=0; i < iSize; i++)
					{
						aResults[i] = new ExampleAddress(aAddresses[i]);
					}
				}
			}
			return aResults;
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns a comment describing the example address
		/// </summary>
		public string Comment
		{
			get
			{
				return m_sComment;
			}
		}

		/// <summary>
		/// Returns the formatted example address
		/// </summary>
		public AddressLine[] AddressLines
		{
			get
			{
				return m_Address.AddressLines;
			}
		}
	}
}
