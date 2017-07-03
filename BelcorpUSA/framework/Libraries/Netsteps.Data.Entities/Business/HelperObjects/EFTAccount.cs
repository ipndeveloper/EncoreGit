
namespace NetSteps.Data.Entities.Business.HelperObjects
{
	public class EFTAccount
	{
	    public int DisbursementProfileID { get; set; }

	    public bool Enabled { get; set; }

	    public string Name { get; set; }

	    public string RoutingNumber { get; set; }
		
        public string AccountNumber { get; set; }
		
        public string BankName { get; set; }
		
        public string BankPhone { get; set; }
		
        public string BankAddress1 { get; set; }
		
        public string BankAddress2 { get; set; }
		
        public string BankAddress3 { get; set; }
		
        public string BankCity { get; set; }
		
        public string BankState { get; set; }
		
        public string BankZip { get; set; }
		public string BankCountry { get; set; }
		public string BankCounty { get; set; }
        public Constants.BankAccountTypeEnum AccountType { get; set; }
		
        public int PercentToDeposit { get; set; }

        public int AccountID { get; set; }

        public int DisbursementTypeID { get; set; }

        public int BankID { get; set; }

        public string BankAgency { get; set; }

        public bool IsEmpty()
        {
            return Extensions.IAddressExtensions.IsEmpty(new Address
            {
                PhoneNumber = BankPhone,
                Address1 = BankAddress1,
                Address2 = BankAddress2,
                Address3 = BankAddress3,
                City = BankCity,
                State = BankState,
                PostalCode = BankZip
            });
        }
	}
}