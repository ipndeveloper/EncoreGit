using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Common.Utility
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Taken from Silverlight project to import Contact exported from 'Outlook Contact Export CSV' formatted doc. 
	/// Created: 12-17-2010
	/// </summary>
	public class OutlookContactsParseHelper
	{
		private enum CSVParseCharType
		{
			Quote = 0,
			Comma = 1,
			Newline = 2,
			NonFlaggedChar = 3
		}
		public static List<List<string>> CSVParser(string csvContents)
		{
			List<List<string>> result = new List<List<string>>();

			bool insideQuote = false;

			List<string> row = new List<string>();
			string cell = string.Empty;

			char preChar = csvContents[0];

			for (int i = 0; i < csvContents.Length; i++)
			{
				char c = csvContents[i];
				CSVParseCharType charType = GetCSVParseCharType(c);

				if (!insideQuote)
				{
					#region Handle Normal State

					if (charType == CSVParseCharType.NonFlaggedChar)
					{
						cell += c;
					}
					else if (charType == CSVParseCharType.Comma)
					{
						row.Add(cell);
						cell = string.Empty;
					}
					else if (charType == CSVParseCharType.Newline)
					{
						row.Add(cell);
						cell = string.Empty;
						result.Add(row);
						row = new List<string>();

						char nextChar = csvContents[i + 1];
						CSVParseCharType nextCharType = GetCSVParseCharType(nextChar);
						if (nextCharType == CSVParseCharType.Newline)
						{
							i++;
						}
					}
					else //if (charType == CSVParseCharType.Quote)
					{
						insideQuote = true;
					}

					#endregion
				}
				else
				{
					#region Handle Inside Quote State

					if (charType == CSVParseCharType.Quote)
					{
						char nextChar = csvContents[i + 1];
						CSVParseCharType nextCharType = GetCSVParseCharType(nextChar);
						if (nextCharType == CSVParseCharType.Quote)
						{
							cell += c;
							i++;
						}
						else
						{
							insideQuote = false;
						}
					}
					else if (charType == CSVParseCharType.Newline)
					{
						cell += Environment.NewLine;

						char nextChar = csvContents[i + 1];
						CSVParseCharType nextCharType = GetCSVParseCharType(nextChar);
						if (nextCharType == CSVParseCharType.Newline)
						{
							i++;
						}
					}
					else //if (charType == CSVParseCharType.NonFlaggedChar || charType == CSVParseCharType.Comma)
					{
						cell += c;
					}

					#endregion
				}

				#region Handle End of String without a Newline

				if (i == csvContents.Length - 1 && charType != CSVParseCharType.Newline)
				{
					row.Add(cell);
					cell = string.Empty;
					result.Add(row);
					row = new List<string>();
				}

				#endregion
			}

			return result;
		}
		private static CSVParseCharType GetCSVParseCharType(char charInput)
		{
			if (charInput == '"')
				return CSVParseCharType.Quote;
			else if (charInput == ',')
				return CSVParseCharType.Comma;
			else if ('\r'.Equals(charInput) || '\n'.Equals(charInput))
				return CSVParseCharType.Newline;
			else
				return CSVParseCharType.NonFlaggedChar;
		}

        public static List<List<string>> RemoveEmptyRows(List<List<string>> rows)
        {
            List<List<string>> trimmedRows = new List<List<string>>();

            foreach (List<string> row in rows)
            {
                if (row.Any(x => !String.IsNullOrWhiteSpace(x)))
                {
                    trimmedRows.Add(row);
                    continue;
                }
            }
            return trimmedRows;
        }

	}
}
