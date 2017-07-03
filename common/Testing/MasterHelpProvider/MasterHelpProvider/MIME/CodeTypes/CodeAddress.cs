using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME.CodeTypes
{
    public class CodeAddress:CodeBase
    {
        public CodeAddress()
        {
        }

        protected override string[] FoldCharacters
        {
            get
            {
                string []TempArray={",",":"};
                return TempArray;
            }
        }

        protected override bool IsAutoFold
        {
            get
            {
                return true;
            }
        }

        protected override bool DelimeterNeeded
        {
            get
            {
                return true;
            }
        }

        protected override char[] DelimeterCharacters
        {
            get
            {
                char[] TempDelimeterArray = { '(', ')', '<', '>', '"' };
                return TempDelimeterArray;
            }
        }
    }
}