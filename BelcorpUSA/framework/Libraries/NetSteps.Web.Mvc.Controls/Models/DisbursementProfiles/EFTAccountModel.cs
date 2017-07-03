using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles
{
    public class EFTAccountModel
    {
        public string AccountNumber { get; set; }

        public BankAccountKind AccountType { get; set; }

        public string BankAddress1 { get; set; }

        public string BankAddress2 { get; set; }

        public string BankAddress3 { get; set; }

        public string BankCity { get; set; }

        public string BankCountry { get; set; }

        public string BankCounty { get; set; }

        public string BankName { get; set; }

        public string BankPhone { get; set; }

        public string BankState { get; set; }

        public string BankZip { get; set; }

        public int DisbursementProfileId { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public int PercentToDeposit { get; set; }

        public string RoutingNumber { get; set; }

        public int? BankId { get; set; }
    }

    public static class EFTAccountModelExtensions
    {
        public static IEFTAccount Convert(this EFTAccountModel model)
        {
            var c = Create.New<IEFTAccount>();
            c.AccountNumber = model.AccountNumber;
            c.AccountType = model.AccountType;
            c.BankAddress1 = model.BankAddress1;
            c.BankAddress2 = model.BankAddress2;
            c.BankAddress3 = model.BankAddress3;
            c.BankCity = model.BankCity;
            c.BankCountry = model.BankCountry;
            c.BankCounty = model.BankCounty;
            c.BankName = model.BankName;
            c.BankPhone = model.BankPhone;
            c.BankState = model.BankState;
            c.BankZip = model.BankZip;
            c.DisbursementProfileId = model.DisbursementProfileId;
            c.IsEnabled = model.Enabled;
            c.Name = model.Name;
            c.PercentToDeposit = model.PercentToDeposit;
            c.RoutingNumber = model.RoutingNumber;
            
            return c;
        }
    }
}
