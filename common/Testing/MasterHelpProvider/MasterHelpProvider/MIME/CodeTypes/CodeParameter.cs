using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME.CodeTypes
{
    public class CodeParameter:CodeBase
    {
        public CodeParameter()
        {
        }

        protected override string[] FoldCharacters
        {
            get
            {
                string[] TempCharacters = { ";" };
                return TempCharacters;
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
                char[] DelimeterCharactersUsing = { '(', ')', '<', '>', '"' };
                return DelimeterCharactersUsing;
            }
        }
    }
}