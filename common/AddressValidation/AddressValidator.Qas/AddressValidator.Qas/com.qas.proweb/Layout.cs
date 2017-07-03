/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > Layout.cs
/// Layout details (for address formatting)


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate layout data
	/// </summary>
	public class Layout
	{
		// -- Private Members --


		private string m_sName = null;
		private string m_sComment = null;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="l"></param>
		public Layout(QALayout l)
		{
			m_sName = l.Name;
			m_sComment = l.Comment;
		}

		/// <summary>
		/// Create array from SOAP-layer array
		/// </summary>
		/// <param name="aLayouts"></param>
		/// <returns></returns>
		public static Layout[] CreateArray(QALayout[] aLayouts)
		{
			Layout[] aResults = null;
			if (aLayouts != null)
			{
				int iSize = aLayouts.GetLength(0);
				if (iSize > 0)
				{
					aResults = new Layout[iSize];
					for (int i=0; i < iSize; i++)
					{
						aResults[i] = new Layout(aLayouts[i]);
					}
				}
			}
			return aResults;
		}

		/// <summary>
		/// Returns the Layout which matches the name, otherwise null
		/// </summary>
		/// <param name="aLayouts">Array of layouts to search</param>
		/// <param name="sLayoutName">Layout name to search for</param>
		/// <returns></returns>
		public static Layout FindByName(Layout[] aLayouts, string sLayoutName)
		{
			for (int i=0; i < aLayouts.GetLength(0); i++)
			{
				if (aLayouts[i].Name.Equals(sLayoutName))
				{
					return aLayouts[i];
				}
			}
         return null;
		}

		
		// -- Read-only Properties --


		/// <summary>
		/// Returns the name of the layout
		/// </summary>
		public string Name
		{
			get
			{
				return m_sName;
			}
		}

		/// <summary>
		/// Returns any comment asscoiated with this layout
		/// </summary>
		public string Comment
		{
			get
			{
				return m_sComment;
			}
		}
	}
}
