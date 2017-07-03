using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Email.MIME
{
    public class Attribute
    {
        public Attribute()
        {
        }

        public Attribute(string AttributeText)
        {
            string[] Splitter = { "=" };
            string[] Values = AttributeText.Split(Splitter, StringSplitOptions.None);
            if (Values.Length == 2)
            {
                Name = Values[0];
                Value = Values[1];
            }
            else if (Values.Length > 2)
            {
                Name = Values[0];
                Value = Values[1];
                for (int x = 2; x < Values.Length; ++x)
                {
                    Value += "=" + Values[x];
                }
            }
            else
            {
                Value = Values[0];
            }
            Regex TempReg = new Regex("\r\n*");
            Name = TempReg.Replace(Name, String.Empty);
            TempReg = new Regex("\t*");
            Name = TempReg.Replace(Name, String.Empty);
            TempReg = new Regex(Regex.Escape(" ") + "*");
            Name = TempReg.Replace(Name, String.Empty);
        }


        private string _Name=String.Empty;
        private string _Value=String.Empty;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
