using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME.CodeTypes
{
    public class CodeBase64:Code
    {
        public CodeBase64()
        {
        }

        public override void Decode(string Input, out byte[] Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            Output = System.Convert.FromBase64String(Input);
        }

        public override string Encode(byte[] Input)
        {
            if (Input==null)
                throw new ArgumentNullException("Input can not be null");
            string TempString = System.Convert.ToBase64String(Input);
            int MAX = 76;
            int Index = 0;
            StringBuilder TempBuilder = new StringBuilder();
            while ((Index + MAX) < TempString.Length)
            {
                TempBuilder.AppendFormat("{0}\r\n", TempString.Substring(Index, MAX));
                Index += MAX;
            }
            TempBuilder.AppendFormat("{0}", TempString.Substring(Index, TempString.Length - Index));
            return TempBuilder.ToString();
        }
    }
}