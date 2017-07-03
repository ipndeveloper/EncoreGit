using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utilities.Email.MIME.CodeTypes
{
	public class CodeQP:Code
	{
		public CodeQP()
		{
		}

		public override void Decode(string Input, out byte[] Output)//patch
		{
			if (string.IsNullOrEmpty(Input))
			{
				throw new ArgumentNullException("Input can not be null");
			}

			string CurrentLine=String.Empty;
			MemoryStream MemoryStream=new MemoryStream();
			StringReader Reader=new StringReader(Input);
			try
			{
				while ((CurrentLine = Reader.ReadLine()) != null)
				{
					if (!string.IsNullOrEmpty(CurrentLine))
					{
						DecodeOneLine(MemoryStream, CurrentLine);
					}
					//while (!string.IsNullOrEmpty(CurrentLine))
					//{
					//    DecodeOneLine(MemoryStream, CurrentLine);
					//    CurrentLine = Reader.ReadLine();
					//}
				}
				Output = MemoryStream.ToArray();
			}
			finally
			{
				MemoryStream.Close();
				MemoryStream = null;
				Reader.Close();
				Reader = null;
			}
		}

		protected void DecodeOneLine(MemoryStream Stream,string CurrentLine)
		{
			if (Stream == null || string.IsNullOrEmpty(CurrentLine))
			{
				throw new ArgumentNullException("Input variables can not be null");
			}
			for (int x = 0, y = 0; x < CurrentLine.Length; ++x, ++y)
			{
				byte CurrentByte;
				if (CurrentLine[x] == '=')
				{
					if (x + 2 > CurrentLine.Length) break;
					if (IsHex(CurrentLine[x + 1]) && IsHex(CurrentLine[x + 2]))
					{
						string HexCode = CurrentLine.Substring(x + 1, 2);
						CurrentByte = Convert.ToByte(HexCode, 16);
						x += 2;
					}
					else
					{
						CurrentByte = Convert.ToByte(CurrentLine[++x]);
					}
				}
				else
				{
					CurrentByte = Convert.ToByte(CurrentLine[x]);
				}
				Stream.WriteByte(CurrentByte);
			}
			if (!CurrentLine.EndsWith("="))
			{
				Stream.WriteByte(0x0D);
				Stream.WriteByte(0x0A);
			}
		}

		protected bool IsHex(char Input)
		{
			if((Input >= '0' && Input <= '9') || (Input >= 'A' && Input <= 'F') || (Input >= 'a' && Input <= 'f'))
				return true;
			else
				return false;
		}

		public override string Encode(byte[] Input)
		{
			if (Input == null)
			{
				throw new ArgumentNullException("Input can not be null");
			}
			StringBuilder Output = new StringBuilder();
			foreach (byte Index in Input)
			{
				if ((Index < 33 || Index > 126 || Index == 0x3D)
					&& Index != 0x09 && Index != 0x20 && Index != 0x0D && Index != 0x0A)
				{
					int Code = (int)Index;
					Output.AppendFormat("={0}", Code.ToString("X2"));
				}
				else
				{
					Output.Append(System.Convert.ToChar(Index));
				}
			}
			Output.Replace(" \r", "=20\r", 0, Output.Length);
			Output.Replace("\t\r", "=09\r", 0, Output.Length);
			Output.Replace("\r\n.\r\n", "\r\n=2E\r\n", 0, Output.Length);
			Output.Replace(" ", "=20", Output.Length - 1, 1);
			return FormatEncodedString(Output.ToString());
		}

		protected string FormatEncodedString(String Input)
		{
			string CurrentLine;
			StringReader Reader = new StringReader(Input);
			StringBuilder Builder = new StringBuilder();
			try
			{
				CurrentLine = Reader.ReadLine();
				while(!string.IsNullOrEmpty(CurrentLine))
				{
					int Index = MAX_CHAR_LEN;
					int LastIndex = 0;
					while(Index < CurrentLine.Length)
					{
						if(IsHex(CurrentLine[Index]) && IsHex(CurrentLine[Index-1]) && CurrentLine[Index-2] == '=')
						{
							Index -= 2;
						}
						Builder.Append(CurrentLine.Substring(LastIndex, Index - LastIndex));
						Builder.Append("=\r\n");
						LastIndex = Index;
						Index += MAX_CHAR_LEN;
					}
					Builder.Append(CurrentLine.Substring(LastIndex, CurrentLine.Length - LastIndex));
					Builder.Append("\r\n");
				}
				return Builder.ToString();
			}
			finally
			{
				Reader.Close();
				Reader=null;
				Builder=null;
			}
		}

		protected const int MAX_CHAR_LEN = 75;
	}
}