using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utilities.Email.MIME
{
    public class MIMEHeader
    {
        public MIMEHeader()
        {
        }

        public MIMEHeader(string HeaderText)
        {
            if(string.IsNullOrEmpty(HeaderText))
                throw new ArgumentNullException("Header text can not be null");
            StringReader Reader=new StringReader(HeaderText);
            try
            {
                string LineRead=Reader.ReadLine();
                string Field=LineRead+"\r\n";
                while(!string.IsNullOrEmpty(LineRead))
                {
                    LineRead=Reader.ReadLine();
                    if (!string.IsNullOrEmpty(LineRead) && (LineRead[0].Equals(' ') || LineRead[0].Equals('\t')))
                    {
                        Field += LineRead + "\r\n";
                    }
                    else
                    {
                        Fields.Add(new Field(Field));
                        Field = LineRead + "\r\n";
                    }
                }
            }
            finally
            {
                Reader.Close();
                Reader=null;
            }
        }

        private List<Field> _Fields=new List<Field>();
        public List<Field>Fields
        {
            get{return _Fields;}
            set{_Fields=value;}
        }

        public Field this[string Key]
        {
            get
            {
                foreach (Field TempField in Fields)
                {
                    if (TempField.Name.Equals(Key,StringComparison.InvariantCultureIgnoreCase))
                    {
                        return TempField;
                    }
                }
                return null;
            }
        }

    }
}