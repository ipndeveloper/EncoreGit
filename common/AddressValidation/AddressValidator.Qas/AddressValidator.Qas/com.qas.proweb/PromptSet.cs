/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > PromptSet.cs
/// Prompt set details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// Simple class to encapsulate data representing a search prompt set
	/// </summary>
	public class PromptSet
	{
		// -- Public Constants --


		/// <summary>
		/// Enumeration of available search prompt sets
		/// </summary>
		public enum Types
		{
			OneLine		= PromptSetType.OneLine,
			Default		= PromptSetType.Default,
			Generic		= PromptSetType.Generic,
			Optimal		= PromptSetType.Optimal,
			Alternate	= PromptSetType.Alternate,
			Alternate2	= PromptSetType.Alternate2,
			Alternate3	= PromptSetType.Alternate3
		}


		// -- Private Members --


		private bool m_bDynamic;
		private PromptLine[] m_aLines;


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		/// <param name="t"></param>
		public PromptSet(QAPromptSet tPromptSet)
		{
			m_bDynamic = tPromptSet.Dynamic;

			m_aLines = null;
			if (tPromptSet.Line != null)
			{
				int iSize = tPromptSet.Line.GetLength(0);
				if (iSize > 0)
				{
					m_aLines = new PromptLine[iSize];
					for (int i=0; i < iSize; i++)
					{
						m_aLines[i] = new PromptLine(tPromptSet.Line[i]);
					}
				}
			}
		}

		/// <summary>
		/// Returns a <code>String[]</code> of prompts (from the search prompt line array)
		/// </summary>
		/// <returns></returns>
		public String[] GetLinePrompts()
		{
			int iSize = m_aLines.GetLength(0);
			String[] asResults = new String[iSize];
			for (int i=0; i < iSize; i++)
			{
				asResults[i] = m_aLines[i].Prompt;
			}
			return asResults;
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns whether dynamic searching should be used (submitting the search as they type)
		/// </summary>
		public bool IsDynamic
		{
			get
			{
				return m_bDynamic;
			}
		}

		/// <summary>
		/// Returns the array of search prompt lines that make up this search prompt set
		/// </summary>
		public PromptLine[] Lines
		{
			get
			{
				return m_aLines;
			}
		}
	}
}
