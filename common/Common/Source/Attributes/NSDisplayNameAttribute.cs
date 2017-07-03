using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Globalization;

namespace System.ComponentModel
{
    public class NSDisplayNameAttribute : DisplayNameAttribute
    {
        public string TermName { get; private set; }

        public NSDisplayNameAttribute(string termName) : this(termName, termName) { }

        public NSDisplayNameAttribute(string termName, string displayName)
        {
            this.TermName = termName;
            this.DisplayNameValue = displayName;
        }

        public override string DisplayName
        {
            get
            {
                return Translation.GetTerm(this.TermName, base.DisplayName);
            }
        }
    }
}
