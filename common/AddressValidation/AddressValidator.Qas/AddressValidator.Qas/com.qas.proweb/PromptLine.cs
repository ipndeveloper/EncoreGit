/// QAS Pro On Demand > (c) QAS Ltd > www.qas.com
/// 
/// Common Classes > PromptLine.cs
/// Prompt line details


using System;
using com.qas.proweb.soap;


namespace com.qas.proweb
{
	/// <summary>
	/// This class encapsulates one line of a search prompt set.
	/// </summary>
	public class PromptLine
	{
		// -- Private Members --


		private string m_sPrompt;
		private string m_sExample;
		private int m_iSuggestedInputLength = 0; // positive integer


		// -- Public Methods --


		/// <summary>
		/// Construct from SOAP-layer object
		/// </summary>
		public PromptLine(com.qas.proweb.soap.PromptLine t)
		{
			m_sPrompt = t.Prompt;
			m_sExample = t.Example;
			m_iSuggestedInputLength = System.Convert.ToInt32(t.SuggestedInputLength);
		}


		// -- Read-only Properties --


		/// <summary>
		/// Returns the prompt for this input line (e.g. "Town" or "Street")
		/// </summary>
		public string Prompt
		{
			get
			{
				return m_sPrompt;
			}
		}

		/// <summary>
		/// Returns an example of what is expected for this input line (e.g. "London")
		/// </summary>
		public string Example
		{
			get
			{
				return m_sExample;
			}
		}

		/// <summary>
		/// Returns the length in characters that is suggested for an input field for this line
		/// </summary>
		public int SuggestedInputLength
		{
			get
			{
				return m_iSuggestedInputLength;
			}
		}
	}
}
