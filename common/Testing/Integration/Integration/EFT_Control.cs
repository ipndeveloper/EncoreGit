using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class EFT_Control : Control<Div>
    {
        private CheckBox _enabled;
        private TextField _name, _routing, _account, _bank, _deposit;
        private Address_Control _address;
        private SelectList _accountType;
        private Phone_Control _phone;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _enabled = Element.GetElement<CheckBox>(new Param("chkEnabledAccount", AttributeName.ID.Id, RegexOptions.None));
            _name = Element.GetElement<TextField>(new Param("txtNameAccount", AttributeName.ID.Id, RegexOptions.None));
            _routing = Element.GetElement<TextField>(new Param("txtRoutingNumberAccount", AttributeName.ID.Id, RegexOptions.None));
            _account = Element.GetElement<TextField>(new Param("txtAccountNumberAccount", AttributeName.ID.Id, RegexOptions.None));
            _bank = Element.GetElement<TextField>(new Param("txtBankNameAccount", AttributeName.ID.Id, RegexOptions.None));
            _phone = Element.As<Phone_Control>();
            _address = Element.As<Address_Control>();
            _accountType = Element.GetElement<SelectList>(new Param("accountTypeAccount", AttributeName.ID.Id, RegexOptions.None));
            _deposit = Element.GetElement<TextField>(new Param("percentToDepositAccount", AttributeName.ID.Id, RegexOptions.None));
        }

        public void EnterDetails(EFTProfile eft)
        {
            _enabled.CustomSetCheckBox();
            _name.CustomSetTextQuicklyHelper(eft.AccountName);
            _routing.CustomSetTextQuicklyHelper(eft.Routing);
            _account.CustomSetTextQuicklyHelper(eft.AccountNumber);
            _bank.CustomSetTextQuicklyHelper(eft.Bank);
            _phone.EnterPhone(eft.BankPhone);
            _address.EnterAddress(eft.BankAddress);
            _accountType.CustomSelectDropdownItem(eft.AccountType);
            _deposit.CustomSetTextQuicklyHelper(eft.DepositPercentage.ToString());
        }

        public bool ValidateEFT(EFTProfile eft)
        {
            bool valid = Compare.CustomCompare<bool>(true, CompareID.Equal, _enabled.CustomChecked(), "Enable");
            if(!Compare.CustomCompare<string>(eft.AccountName, CompareID.Equal, _name.CustomGetText(), "Name"))
                valid = false;
            if(!Compare.CustomCompare<string>(eft.Routing, CompareID.Equal, _routing.CustomGetText(), "Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(eft.AccountNumber.Substring(8), CompareID.Equal, _account.CustomGetText().Substring(8), "Account number"))
                valid = false;
            if (!Compare.CustomCompare<string>(eft.Bank, CompareID.Equal, _bank.CustomGetText(), "Bank"))
                valid = false;
            if (!_phone.ValidatePhone(eft.BankPhone))
                valid = false;
            if (!_address.ValidateAddress(eft.BankAddress))
                valid = false;
            if (!Compare.CustomCompare<string>(eft.AccountType, CompareID.Equal, _accountType.CustomGetSelectedItem(), "Account type"))
                valid = false;
            if(!Compare.CustomCompare<string>(eft.DepositPercentage.ToString(), CompareID.Equal, _deposit.CustomGetText(), "Deposit %"))
                valid = false;
            return valid;
        }
    }
}
