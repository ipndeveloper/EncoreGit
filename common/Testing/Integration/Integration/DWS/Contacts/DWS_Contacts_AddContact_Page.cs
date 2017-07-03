using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public class DWS_Contacts_AddContact_Page : DWS_Contacts_Base_Page
    {
        private TextField txtFirstName;
        private TextField txtMiddleName;
        private TextField txtLastName;
        private TextField txtEmail;
        private TextField txtDOBMonth;
        private TextField txtDOBDay;
        private TextField txtDOBYear;
        private TextField txtAddressOne;
        private TextField txtAddressTwo;
        private TextField txtPostalCode;
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this.txtFirstName = Document.GetElement<TextField>(new Param("firstName"));
            this.txtMiddleName = Document.GetElement<TextField>(new Param("middleName"));
            this.txtLastName = Document.GetElement<TextField>(new Param("lastName"));
            this.txtDOBMonth = Document.GetElement<TextField>(new Param("txtDOBMonth"));
            this.txtDOBDay = Document.GetElement<TextField>(new Param("txtDOBDay"));
            this.txtDOBYear = Document.GetElement<TextField>(new Param("txtDOBYear"));
            this.txtAddressOne = Document.GetElement<TextField>(new Param("address1"));
            this.txtAddressTwo = Document.GetElement<TextField>(new Param("address2"));
            this.txtPostalCode = Document.GetElement<TextField>(new Param("zip"));
            this._save = Document.GetElement<Link>(new Param("btnSave"));
            this.txtEmail = Document.GetElement<TextField>(new Param("email"));
        }
        /// <summary>
        /// Enter prospect complete name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="middleName">Middle name.</param>
        /// <param name="lastName">Last name.</param>
        public void EnterProspectCompleteName(string firstName, string middleName, string lastName)
        {
            this.txtFirstName.CustomSetTextHelper(firstName);
            this.txtMiddleName.CustomSetTextHelper(middleName);
            this.txtLastName.CustomSetTextHelper(lastName);
        }

        /// <summary>
        /// Set data for DOB fields.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        public void EnterDOBFields(string dateOfBirth)
        {
            this.txtDOBMonth.CustomSetTextHelper(dateOfBirth.Split('/')[0]);
            this.txtDOBDay.CustomSetTextHelper(dateOfBirth.Split('/')[1]);
            this.txtDOBYear.CustomSetTextHelper(dateOfBirth.Split('/')[2]);
        }

        /// <summary>
        /// Enter main address under DWS prospect.
        /// </summary>
        /// <param name="addressLineOne">Address line one.</param>
        /// <param name="addressLineTwo">Address line two.</param>
        /// <param name="postalCode">Postal code.</param>
        public void EnterMainAddressDetails(string addressLineOne, string addressLineTwo, string postalCode)
        {
            // Enter address and postal code. 
            this.txtAddressOne.CustomSetTextHelper(addressLineOne);
            this.txtAddressTwo.CustomSetTextHelper(addressLineTwo);
            this.txtPostalCode.CustomSetTextHelper(postalCode);
            this.txtPostalCode.CustomRunScript(Util.strKeyUp);
        }

        /// <summary>
        /// Click save contact link.
        /// </summary>
        public void ClickSave(int? timeout = null)
        {
            this._save.CustomClick(timeout);
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
