using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public class Field
    {
        public Field()
        {
        }

        public Field(string FieldText)
        {
            if (string.IsNullOrEmpty(FieldText))
                throw new ArgumentNullException();

            int Index = FieldText.IndexOf(':');
            if (Index != -1)
                Name = FieldText.Substring(0, Index);

            ++Index;
            FieldText = FieldText.Substring(Index, FieldText.Length - Index).Trim();
            string[] Splitter = { ";" };
            string[] Attributes = FieldText.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string AttributeText in Attributes)
            {
                Code TempCode = CodeManager.Instance[Name];
                if (TempCode != null)
                {
                    string TempText = String.Empty;
                    TempCode.Decode(AttributeText, out TempText);
                    this.Attributes.Add(new Attribute(TempText));
                    CharacterSet = TempCode.CharacterSet;
                }
                else
                {
                    this.Attributes.Add(new Attribute(AttributeText));
                }
            }
        }

        private string _Name=String.Empty;
        private List<Attribute> _Attributes=new List<Attribute>();
        private string _CharacterSet=String.Empty;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public List<Attribute> Attributes
        {
            get { return _Attributes; }
            set { _Attributes = value; }
        }

        public string CharacterSet
        {
            get { return _CharacterSet; }
            set { _CharacterSet = value; }
        }

        public string this[string Key]
        {
            get
            {
                foreach (Attribute TempAttribute in Attributes)
                {
                    if (TempAttribute.Name.Equals(Key,StringComparison.InvariantCultureIgnoreCase))
                    {
                        return TempAttribute.Value;
                    }
                }
                return String.Empty;
            }
        }
    }
}
