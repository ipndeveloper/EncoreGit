using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;

namespace TestMasterHelpProvider
{
	public class CsvDataLoader
	{
		#region Fields

		private static string __columnDelimiter = ",";
		private static string __textDelimiter = "\"";
		private static string __nullValue = "\"NULL\"";

		private IList<NameValueCollection> _data;
		private IList<string> _columns;
		private Assembly _withEmbeddedResource;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of a null value in the CSV file.
		/// </summary>
		public static string NullValue
		{
			get { return CsvDataLoader.__nullValue; }
			set { CsvDataLoader.__nullValue = value; }
		}

		/// <summary>
		/// Gets or sets the column delimiter.
		/// </summary>
		public static string ColumnDelimiter
		{
			get { return CsvDataLoader.__columnDelimiter; }
			set { CsvDataLoader.__columnDelimiter = value; }
		}

		/// <summary>
		/// Gets or sets the text delimiter.
		/// </summary>
		public static string TextDelimiter
		{
			get { return CsvDataLoader.__textDelimiter; }
			set { CsvDataLoader.__textDelimiter = value; }
		}

		/// <summary>
		/// Gets the columns of the CSV file.
		/// </summary>
		public IList<string> Columns
		{
			get { return _columns; }
		}

		/// <summary>
		/// Gets a specific row of the data.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public NameValueCollection this[int index]
		{
			get { return _data[index]; }
		}

		/// <summary>
		/// Gets the total number of rows of data.
		/// </summary>
		public int Count
		{
			get { return _data.Count; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of CsvDataLoader.
		/// </summary>
		public CsvDataLoader()
		{
			_data = new List<NameValueCollection>();
			_columns = new List<string>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<NameValueCollection> GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		/// <summary>
		/// Loads a CSV resource file.
		/// </summary>
		/// <param name="resourceLocation"></param>
		/// <param name="isEmbedded"></param>
		public void Load(string resourceLocation, bool isEmbedded = false, Assembly withEmbeddedResource = null)
		{
			_withEmbeddedResource = withEmbeddedResource;

			if (isEmbedded)
			{
				LoadEmbeddedResource(resourceLocation);
			}
			else
			{
				LoadExternalResource(resourceLocation);
			}
		}

		/// <summary>
		/// Loads an external resource.
		/// </summary>
		/// <param name="resourceLocation"></param>
		private void LoadExternalResource(string resourceLocation)
		{
			using (TextReader reader = new StreamReader(resourceLocation))
			{
				LoadColumns(reader);

				if (Columns.Count > 0)
				{
					LoadRows(reader);
				}
			}
		}

		/// <summary>
		/// Loads an embedded resource.
		/// </summary>
		/// <param name="resourceLocation"></param>
		private void LoadEmbeddedResource(string resourceLocation)
		{
			if (_withEmbeddedResource == null)
			{
				_withEmbeddedResource = Assembly.GetExecutingAssembly();
			}

			using (TextReader reader = new StreamReader(_withEmbeddedResource.GetManifestResourceStream(resourceLocation)))
			{
				LoadColumns(reader);

				if (Columns.Count > 0)
				{
					LoadRows(reader);
				}
			}
		}

		/// <summary>
		/// Loads the columns.
		/// </summary>
		/// <param name="reader"></param>
		private void LoadColumns(TextReader reader)
		{
			string columnLine = reader.ReadLine();
			string[] columnNames = columnLine.Split(new string[] { CsvDataLoader.__columnDelimiter }, StringSplitOptions.RemoveEmptyEntries);

			if (columnNames.Length > 0)
			{
				foreach (string nextColumnName in columnNames)
				{
					_columns.Add(nextColumnName.Replace(CsvDataLoader.__textDelimiter, String.Empty));
				}
			}
		}

		/// <summary>
		/// Loads the rows into the data collection.
		/// </summary>
		/// <param name="reader"></param>
		private void LoadRows(TextReader reader)
		{
			string nextLine = reader.ReadLine();

			while (!String.IsNullOrEmpty(nextLine))
			{
				string[] columnValues = ParseCsvLine(nextLine);

				if (columnValues.Length > 0)
				{
					if (columnValues.Length == Columns.Count)
					{
						NameValueCollection nextRow = new NameValueCollection();

						for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
						{
							nextRow.Add(Columns[columnIndex], columnValues[columnIndex].Replace(CsvDataLoader.__textDelimiter, String.Empty));
						}

						_data.Add(nextRow);
					}
					else
					{
						throw new FileLoadException("CSV file is not well-formed. Column data must reflect column headers.");
					}
				}

				nextLine = reader.ReadLine();
			}
		}

		/// <summary>
		/// Parses a CSV line into an array of string column values.
		/// </summary>
		/// <param name="csvLine"></param>
		/// <returns></returns>
		private string[] ParseCsvLine(string csvLine)
		{
			IList<string> columnValues = new List<string>();
			int nextIndexOfSeparatorChar = -1;
			int substringLength = 0;
			string substring = String.Empty;

			while ((nextIndexOfSeparatorChar = csvLine.IndexOf(CsvDataLoader.__columnDelimiter)) > -1 || csvLine.Length > 0)
			{
				if (nextIndexOfSeparatorChar == -1)
				{
					substring = csvLine;
				}
				else
				{
					substring = csvLine.Substring(0, nextIndexOfSeparatorChar);
				}

				substringLength = substring.Length + 1;

				if (substring.Equals(CsvDataLoader.__nullValue))
				{
					substring = String.Empty;
				}
				else if (substring.StartsWith(CsvDataLoader.__textDelimiter))
				{
					if (!substring.EndsWith(CsvDataLoader.__textDelimiter))
					{
						StringBuilder fullString = new StringBuilder(String.Concat(substring, CsvDataLoader.__columnDelimiter));

						while (!fullString.ToString().EndsWith(CsvDataLoader.__textDelimiter))
						{
							int startIndex = (nextIndexOfSeparatorChar + 1);
							int nextIndex = csvLine.IndexOf(CsvDataLoader.__columnDelimiter, startIndex);
							string nextSubstring = String.Empty;

							if (nextIndex == -1)
							{
								nextSubstring = csvLine.Substring(startIndex);
							}
							else
							{
								nextSubstring = csvLine.Substring(startIndex, (nextIndex - fullString.ToString().Length));
							}

							fullString.Append(nextSubstring);
						}

						substring = fullString.ToString();
						substringLength = substring.Length + 1;
					}

					substring = substring.Replace(CsvDataLoader.__textDelimiter, String.Empty);
				}

				if (substringLength > (csvLine.Length - 1))
				{
					csvLine = String.Empty;
				}
				else
				{
					csvLine = csvLine.Substring(substringLength);
				}

				columnValues.Add(substring);
			}

			return columnValues.ToArray<string>();
		}

		#endregion
	}
}
