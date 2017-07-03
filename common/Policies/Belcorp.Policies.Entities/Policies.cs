//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Belcorp.Policies.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Policies
    {
        public Policies()
        {
            this.AccountPolicies = new HashSet<AccountPolicies>();
        }
    
        public int PolicyID { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public string VersionNumber { get; set; }
        public Nullable<System.DateTime> DateReleasedUTC { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> IsAcceptanceRequired { get; set; }
        public bool Active { get; set; }
        public Nullable<int> HtmlSectionID { get; set; }
        public Nullable<short> AccountTypeID { get; set; }
        public string TermName { get; set; }
    
        public virtual ICollection<AccountPolicies> AccountPolicies { get; set; }
    }
}
