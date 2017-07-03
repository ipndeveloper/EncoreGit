using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NetSteps.Common
{
	public class DebugTextWriter : TextWriter
	{
		public override void Write(char[] buffer, int index, int count)
		{
			Debug.Write(new String(buffer, index, count));
		}

		public override void Write(string value)
		{
			Debug.Write(value);
		}

		public override Encoding Encoding
		{
			get { return System.Text.Encoding.Default; }
		}
	}
}
