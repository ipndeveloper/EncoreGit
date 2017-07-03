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
    
    public partial class Accounts
    {
        public Accounts()
        {
            this.AccountPolicies = new HashSet<AccountPolicies>();
            this.Accounts1 = new HashSet<Accounts>();
            this.Accounts11 = new HashSet<Accounts>();
        }
    
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public short AccountTypeID { get; set; }
        public short AccountStatusID { get; set; }
        public Nullable<int> PreferedContactMethodID { get; set; }
        public int DefaultLanguageID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string CoApplicant { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> SponsorID { get; set; }
        public Nullable<int> EnrollerID { get; set; }
        public Nullable<System.DateTime> EnrollmentDateUTC { get; set; }
        public Nullable<bool> IsTaxExempt { get; set; }
        public string TaxNumber { get; set; }
        public bool IsEntity { get; set; }
        public string EntityName { get; set; }
        public Nullable<short> AccountStatusChangeReasonID { get; set; }
        public Nullable<System.DateTime> LastRenewalUTC { get; set; }
        public Nullable<System.DateTime> NextRenewalUTC { get; set; }
        public bool ReceivedApplication { get; set; }
        public bool IsTaxExemptVerified { get; set; }
        public Nullable<System.DateTime> DateApplicationReceivedUTC { get; set; }
        public Nullable<System.DateTime> BirthdayUTC { get; set; }
        public Nullable<short> GenderID { get; set; }
        public byte[] DataVersion { get; set; }
        public Nullable<int> ModifiedByUserID { get; set; }
        public System.DateTime DateCreatedUTC { get; set; }
        public Nullable<int> CreatedByUserID { get; set; }
        public Nullable<short> AccountSourceID { get; set; }
        public Nullable<System.DateTime> DateLastModifiedUTC { get; set; }
        public Nullable<System.DateTime> TerminatedDateUTC { get; set; }
        public string TaxGeocode { get; set; }
        public int MarketID { get; set; }
        public string ETLNaturalKey { get; set; }
        public string ETLHash { get; set; }
        public string ETLPhase { get; set; }
        public Nullable<System.DateTime> ETLDate { get; set; }
    
        public virtual ICollection<AccountPolicies> AccountPolicies { get; set; }
        public virtual ICollection<Accounts> Accounts1 { get; set; }
        public virtual Accounts Accounts2 { get; set; }
        public virtual ICollection<Accounts> Accounts11 { get; set; }
        public virtual Accounts Accounts3 { get; set; }
    }
}
