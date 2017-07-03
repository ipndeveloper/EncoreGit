using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public class Code
    {
        public Code()
        {
        }

        private string _CharacterSet = String.Empty;
        public string CharacterSet
        {
            get { return _CharacterSet; }
            set { _CharacterSet = value.Replace("\"",String.Empty); }
        }

        public virtual void Decode(string Input, out byte[] Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");

            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            Output = Encoding.GetEncoding(CharacterSet).GetBytes(Input);
        }

        public virtual void Decode(string Input, out string Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            byte[] TempBytes=null;
            Decode(Input,out TempBytes);
            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            Output=Encoding.GetEncoding(CharacterSet).GetString(TempBytes);
        }

        public virtual string Encode(byte[] Input)
        {
            if (Input==null)
                throw new ArgumentNullException("Input can not be null");

            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            return Encoding.GetEncoding(CharacterSet).GetString(Input, 0, Input.Length);
        }

        public virtual string Encode(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            byte[] TempArray = Encoding.GetEncoding(CharacterSet).GetBytes(Input);
            return Encode(TempArray);
        }
    }
}