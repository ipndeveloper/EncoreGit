namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using NetSteps.Addresses.Common.Models;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Generated;

    using nsDistributor.Areas.Enroll.Models.Shared;
    using NetSteps.Enrollment.Common.Models.Context;
    using NetSteps.Enrollment.Common.Provider;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Web.Mvc.Controls.Models.Enrollment;
    using System.Web.Mvc;
    using NetSteps.Data.Entities.Extensions;

    using NetSteps.Web.Mvc.Extensions;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Entities.EntityModels;
    using NetSteps.Data.Entities.Business;
    using System.Linq;
    using nsDistributor.Models.Shared;

    public class BasicInfoModel : SectionModel
    {
        #region Values
        [NSRequired]
        [NSDisplayName("FirstName", "First Name")]
        public virtual string FirstName { get; set; }

        [NSRequired]
        [NSDisplayName("LastName", "Last Name")]
        public virtual string LastName { get; set; }


        [NSDisplayName("CPF")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateCPF")]
        public virtual CPFModel CPF { get; set; }


        [NSDisplayName("PIS")]
        [CustomValidation(typeof(BasicInfoModel), "ValidatePIS")]
        public virtual PISModel PIS { get; set; }

        [NSRequired]
        [NSDisplayName("RG", "RG")]
        [NSRegularExpression("^[0-9]*$")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateRG")]
        public virtual string RG { get; set; }

        //[NSRequired]
        [NSDisplayName("OrgExp", "Org. Exp")]
        public virtual string OrgExp { get; set; }

        [NSDisplayName("RGIssueDate", "RG issue Date")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateRGIssueDate")]
        public virtual DateModel RGIssueDate { get; set; }

        [NSDisplayName("SpeakWith", "Speak With")]
        public virtual string SpeakWith { get; set; }

        [NSRequired]
        [NSEmail]
        [NSDisplayName("Email")]
        public virtual string Email { get; set; }

        [NSRequired]
        [NSDisplayName("AuthEmailSend", "AuthEmailSend")]
        public virtual bool AuthEmailSend { get; set; }

        [NSRequired]
        [NSDisplayName("AuthNetworkData", "AuthNetworkData")]
        public virtual bool AuthNetworkData { get; set; }

        [NSRequired]
        [NSDisplayName("AuthShareData", "AuthShareData")]
        public virtual bool AuthShareData { get; set; }

        [NSRequired]
        [NSEmail]
        [NSCompare("Email", TermName = "ErrorEmailsDoNotMatch", ErrorMessage = "Email do not match.")]
        [NSDisplayName("EmailConfirmation", "Email Confirmation")]
        public virtual string EmailConfirmation { get; set; }

        [NSRequired]
        [NSDisplayName("Password")]
        public virtual string Password { get; set; }

        [NSRequired]
        [NSCompare("Password", TermName = "ErrorPasswordsDoNotMatch", ErrorMessage = "Passwords do not match.")]
        [NSDisplayName("ConfirmPassword", "Confirm Password")]
        public virtual string ConfirmPassword { get; set; }

        [NSDisplayName("EnrollAs", "Enroll as")]
        public virtual bool IsEntity { get; set; }
        
     
        [NSDisplayName("SSN")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateSSN")]
        public virtual TaxNumberModel SSN { get; set; }

        [NSRequired(Condition = "EnableEntityValidation")]
        [NSDisplayName("EntityName", "Entity Name")]
        public virtual string EntityName { get; set; }

        [NSDisplayName("EIN")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateEIN")]
        public virtual TaxNumberModel EIN { get; set; }

        [NSRequired]
        [NSDisplayName("Gender")]
        public virtual Constants.Gender Gender { get; set; }

        [NSRequired]
        [NSDisplayName("DateOfBirth", "Date of Birth")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateBirthday")]
        public virtual DateModel Birthday { get; set; }

        [NSRequired]
        [NSDisplayName("Nationality", "Nationality")]
        public virtual string Nationality { get; set; }

        [NSRequired]
        [NSDisplayName("MaritalStatus", "Marital Status")]
        public virtual string MaritalStatus { get; set; }

        [NSRequired]
        [NSDisplayName("Occupation", "Occupation")]
        public virtual string Occupation { get; set; }

        //[NSDisplayName("PhoneMainNumber", "Phone Number(Main)")]
        [NSDisplayName("MainPhoneNumber", "Main Phone Number")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateMainPhone")]
        public virtual PhoneModel MainPhone { get; set; }

        //[NSDisplayName("PhoneMobileNumber", "Phone Number(Mobile)")]
        [NSDisplayName("AltPhoneNumber1", "Alternative Phone Number 1")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateAdditionalPhone")]
        public virtual PhoneModel MobilePhone { get; set; }
        public virtual List<ParameterCountryModel> ParameterCountries { get; set; }

        //[NSDisplayName("PhoneCommercialNumber", "Phone Number(Commercial)")]
        [NSDisplayName("AltPhoneNumber2", "Alternative Phone Number 2")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateAdditionalPhone")]
        public virtual PhoneModel CommercialPhone { get; set; }

        //[NSDisplayName("PhoneMessagesNumber", "Phone Number(To leave Messages)")]
        [NSDisplayName("AltPhoneNumber3", "Alternative Phone Number 3")]
        [CustomValidation(typeof(BasicInfoModel), "ValidateAdditionalPhone")]
        public virtual PhoneModel MessagesPhone { get; set; }

        public virtual bool ShowTaxNumber { get; set; }

        // Hidden
        public virtual int CountryID { get; set; }


        public virtual int CustomLanguageID { get; set; }
        public virtual MvcHtmlString MainAddressHtml { get; set; }
        #endregion

        //#region Value Helpers
        public virtual string TaxNumber
        {
            get
            {
                return this.IsEntity ? this.EIN.Value : this.SSN.Value;
            }
        }

        public virtual string MaskedTaxNumber
        {
            get
            {
                return this.TaxNumber.MaskString(4);
            }
        }
        //#endregion

        #region Models

        public virtual BasicAddressModel MainAddress { get; set; }

        public virtual List<AccountPropertyModel> AccountProperties { get; set; }

        #endregion

        #region Infrastructure

        protected const string _passwordPlaceholder = "~.FAKE.~";

        public BasicInfoModel()
        {
            this.MainAddress = new BasicAddressModel();
            this.SSN = new TaxNumberModel();
            this.EIN = new TaxNumberModel();
            this.CPF = new CPFModel();
            this.PIS = new PISModel();
            this.MainPhone = new PhoneModel();
            this.MobilePhone = new AdditionalPhoneModel();
            this.CommercialPhone = new AdditionalPhoneModel();
            this.MessagesPhone = new AdditionalPhoneModel();
            // Value type defaults (will be overwritten by LoadValues() and model binder)
            this.CountryID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
            this.ParameterCountries = new List<ParameterCountryModel>();
        }

        /// <summary>
        /// Validation Rules
        /// </summary>
        public static readonly Predicate<BasicInfoModel>
            EnableSSNValidation = m => !m.IsEntity && m.ShowTaxNumber,
            EnableEntityValidation = m => m.IsEntity && m.ShowTaxNumber;

        public static ValidationResult ValidateBirthday(DateModel birthday, ValidationContext validationContext)
        {
            string errorMessageFormat = null;
            const int MinYears = 18;
            const int MaxYears = 150;

            if (birthday.IsBlank)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }
            else if (!birthday.IsValid)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
            }
            else if (birthday.Date.Value.AddYears(MinYears) > DateTime.Now)
            {
                errorMessageFormat = Translation.GetTerm("ErrorTooYoungToEnroll", "You must be at least {1} to enroll.");
            }
            else if (birthday.Date.Value.AddYears(MaxYears) < DateTime.Now)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
            }

            if (birthday.Date.HasValue)
            {
                DateTime _birthday = birthday.Date.Value;

                DateTime now = DateTime.Today;
                int age = now.Year - _birthday.Year;
                if (now < _birthday.AddYears(age)) age--;

                if (age < 18)// validar que sea mayor de edad 
                {
                    errorMessageFormat = Translation.GetTerm("18Years", "You must be 18 years of age");
                } 
            }


            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName, MinYears, MaxYears);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateSSN(TaxNumberModel ssn, ValidationContext validationContext)
        { 
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == 1)
            { 

                if (!BasicInfoModel.EnableSSNValidation((BasicInfoModel)validationContext.ObjectInstance))
                {
                    return ValidationResult.Success;
                }

                string errorMessageFormat = null;

                if (ssn.IsBlank)
                {
                    errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
                }
                else if (!ssn.IsValid)
                {
                    errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
                }

                if (!string.IsNullOrEmpty(errorMessageFormat))
                {
                    string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateCPF(CPFModel cpf, ValidationContext validationContext)
        {
            string errorMessageFormat = null;
            Boolean isValidCPF = ValidarCPF(cpf.Value == null ? "" : cpf.Value);
            if (cpf.IsBlank || cpf.Value==null)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }
            if (errorMessageFormat == null)
            {
                if (!isValidCPF)
                {
                    errorMessageFormat = Translation.GetTerm("CpFisInvalid", "the value of CPF is incorrect");
                }
                else
                {

                    if (cpf.Value != null)
                    {
                        IEnrollmentContext<EnrollmentKitConfig> _enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext() as IEnrollmentContext<EnrollmentKitConfig>;

                        int AccountID = 0;
                        if (_enrollmentContext.EnrollingAccount != null)
                            if (_enrollmentContext.EnrollingAccount.AccountID != null)
                                AccountID = _enrollmentContext.EnrollingAccount.AccountID;
                        Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaCPF(cpf.Value, AccountID);
                        if (dcResultado.Count > 1)
                        {
                            errorMessageFormat = Translation.GetTerm("CPFIsRegistered", "CPF entered already registered");
                        }
                    }

                }
            }
            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateRG(string RG  , ValidationContext validationContext)
        {
            string errorMessageFormat = null;

            if (RG == null)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }

            if (errorMessageFormat == null)
            {
                if ((RG == null ? "" : RG).Length == 0)
                {
                    errorMessageFormat = Translation.GetTerm("RGIsInvalid", "the value of RG is incorrect");
                }
                /*CS.02JUL2016.Inicio.Comentado.Quitar Validacion si ya se encuentra registrado*/
                //if ((RG == null ? "" : RG).Length > 0)
                //{
                //    IEnrollmentContext<EnrollmentKitConfig> _enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext() as IEnrollmentContext<EnrollmentKitConfig>;

                //    int AccountID = 0;
                //    if (_enrollmentContext.EnrollingAccount != null)
                //        if (_enrollmentContext.EnrollingAccount.AccountID != null)
                //            AccountID = _enrollmentContext.EnrollingAccount.AccountID;

                //    Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(RG, AccountID);
                //    if (dcResultado.Count > 0)
                //    {
                //        errorMessageFormat = Translation.GetTerm("RGIsRegistered", "RG entered already registered");
                //    }
                //}
                /*CS.02JUL2016.Fin.Comentado.Quitar Validacion si ya se encuentra registrado*/
                if (!string.IsNullOrEmpty(errorMessageFormat))
                {
                    string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
            
            
        }

        public static ValidationResult ValidatePIS(PISModel pis, ValidationContext validationContext)
        {

            if(string.IsNullOrEmpty(pis.Value))
                return ValidationResult.Success;

            string errorMessageFormat = null;

            if (pis.IsBlank || pis.Value == null)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }
            Boolean isValidPIS = ValidarPIS(pis.Value == null ? "" : pis.Value);

            if (errorMessageFormat == null)
            {
                if (!isValidPIS && (pis.Value == null ? "" : pis.Value).Length > 0)
                {
                    errorMessageFormat = Translation.GetTerm("PISisInvalid", "the value of PIS is incorrect");
                }
                else
                {
                    if (isValidPIS)
                    {
                        IEnrollmentContext<EnrollmentKitConfig> _enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext() as IEnrollmentContext<EnrollmentKitConfig>;
                        /*CS.29JUN2016.Inicio.Obtener AccountID*/
                        int AccountID = 0;
                        if (_enrollmentContext.EnrollingAccount != null)
                            if (_enrollmentContext.EnrollingAccount.AccountID != null)
                                AccountID = _enrollmentContext.EnrollingAccount.AccountID;
                        /*CS.29JUN2016.Fin.Obtener AccountID*/
                        Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(pis.Value, AccountID);
                        if (dcResultado.Count > 0)
                        {
                            errorMessageFormat = Translation.GetTerm("PisIsRegistered", "PIS entered already registered");
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }




            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEIN(TaxNumberModel ein, ValidationContext validationContext)
        {
            if (!BasicInfoModel.EnableEntityValidation((BasicInfoModel)validationContext.ObjectInstance))
            {
                return ValidationResult.Success;
            }

            string errorMessageFormat = null;

            if (ein.IsBlank)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }
            else if (!ein.IsValid)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
            }

            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateMainPhone(PhoneModel phone, ValidationContext validationContext)
        {
            string errorMessageFormat = null;

            if (phone.IsBlank)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldRequired", "{0} is required.");
            }
            else if (!phone.IsValid)
            {
                errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
            }

            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateAdditionalPhone(PhoneModel phone, ValidationContext validationContext)
        {
            string errorMessageFormat = null;

            if (!phone.IsBlank)
            {
                if (!phone.IsValid)
                {
                    if (phone.Substrings[0] != null && !phone.Substrings[0].Text.StartsWith("0"))
                        errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
                }
            }

            if (!string.IsNullOrEmpty(errorMessageFormat))
            {
                string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateRGIssueDate(DateModel RGIssueDate, ValidationContext validationContext)
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == 73)
            {
                string errorMessageFormat = null;

                if (!RGIssueDate.IsBlank && !RGIssueDate.IsValid)
                {
                    errorMessageFormat = Translation.GetTerm("ErrorFieldInvalid", "{0} is invalid.");
                }

                if (!string.IsNullOrEmpty(errorMessageFormat))
                {
                    string errorMessage = string.Format(errorMessageFormat, validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }


        public virtual BasicInfoModel LoadValues(
            int countryID,
            Account account,
            IAddress mainAddress,
            bool forcePasswordChange,
            bool showTaxNumber)
        {
            this.CountryID = countryID;
            this.FirstName = account.FirstName;
            this.LastName = account.LastName;
            this.Email = account.EmailAddress;
            this.ShowTaxNumber = showTaxNumber;
            CustomLanguageID = account.DefaultLanguageID;
            // If they already have a password, we use a placeholder
            if (!forcePasswordChange && account.User != null && !string.IsNullOrEmpty(account.User.PasswordHash))
            {
                this.Password = this.ConfirmPassword = _passwordPlaceholder;
            }
            this.IsEntity = account.IsEntity;
            string CPF = string.Empty;
            string PIS = string.Empty;
            DateTime? issueDate = null;

            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == 1)
            {

                this.IsEntity = account.IsEntity;
                if (account.IsEntity)
                {
                    this.EntityName = account.EntityName;
                    this.SSN.LoadValues(string.Empty, false, this.CountryID);
                    this.EIN.LoadValues(account.DecryptedTaxNumber, true, this.CountryID);
                }
                else
                {
                    this.EntityName = string.Empty;
                    this.SSN.LoadValues(account.DecryptedTaxNumber, false, countryId);
                    this.EIN.LoadValues(string.Empty, true, this.CountryID);
                }

                this.Gender = (ConstantsGenerated.Gender)(account.GenderID ?? 0);
                this.Birthday = new DateModel { Date = account.Birthday };
                this.MainPhone.LoadValues(account.MainPhone.RemoveNonNumericCharacters(), this.CountryID);

                this.MainAddress.LoadValues(countryID, mainAddress);

            }
            else
            {
                if (account != null)
                {
                    if (account.AccountID > 0)
                    {
                        AccountSuppliedIDsBusinessLogic metodo = new AccountSuppliedIDsBusinessLogic();
                        List<AccountSuppliedIDsTable> salidalistaAccountSuppliedIDs = new List<AccountSuppliedIDsTable>();
                        AccountSuppliedIDsParameters entradaAccountSuppliedIDs = new AccountSuppliedIDsParameters();
                        entradaAccountSuppliedIDs.AccountID = account.AccountID;
                        salidalistaAccountSuppliedIDs = metodo.GetAccountSuppliedIDByAccountID(entradaAccountSuppliedIDs);
                        if (salidalistaAccountSuppliedIDs.Count() > 0)
                        {
                            CPF = salidalistaAccountSuppliedIDs.Where(donde => donde.Name == "CPF").ElementAt(0).AccountSuppliedIDValue;
                            PIS = salidalistaAccountSuppliedIDs.Where(donde => donde.Name == "PIS").ElementAt(0).AccountSuppliedIDValue;
                            if (salidalistaAccountSuppliedIDs.Where(donde => donde.Name == "RG").Count() > 0)
                                issueDate = salidalistaAccountSuppliedIDs.Where(donde => donde.Name == "RG").ElementAt(0).IDExpeditionIDate;
                        }
                    }
                }

                this.CPF.LoadValues(CPF, this.CountryID);
                this.PIS.LoadValues(PIS, this.CountryID);
                this.RGIssueDate = new DateModel { Date = issueDate };
                /*CS.20JUN2016.Fin.Obtener AccountSuppliedIDs*/

                this.Gender = (ConstantsGenerated.Gender)(account.GenderID ?? 2);
                this.Birthday = new DateModel { Date = account.Birthday };

                this.MainPhone.LoadValues(account.MainPhone.RemoveNonNumericCharacters(), this.CountryID);
                //this.MobilePhone.LoadValues(account.CellPhone.RemoveNonNumericCharacters(), this.CountryID);
                //this.CommercialPhone.LoadValues(account.WorkPhone.RemoveNonNumericCharacters(), this.CountryID);
                //this.MessagesPhone.LoadValues(account.TextPhone.RemoveNonNumericCharacters(), this.CountryID);

                //this.MainPhone.LoadValues("0", this.CountryID);//.LoadValues(account.MainPhone.RemoveNonNumericCharacters(), this.CountryID);
                this.MobilePhone.LoadValues("0", this.CountryID);//.LoadValues(account.CellPhone.RemoveNonNumericCharacters(), this.CountryID);
                this.CommercialPhone.LoadValues("0", this.CountryID);//.LoadValues(account.WorkPhone.RemoveNonNumericCharacters(), this.CountryID);
                this.MessagesPhone.LoadValues("0", this.CountryID);//.LoadValues(account.TextPhone.RemoveNonNumericCharacters(), this.CountryID);

                //mainAddress.CountryID = (int)Constants.Country.Brazil;

                this.MainAddress.LoadValues(countryID, mainAddress, false);
                this.MainAddressHtml = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();
                //this.MainAddress.LoadValues(countryID, mainAddress);
            }  
            return this;
        }

        public virtual BasicInfoModel LoadResources()
        {
            this.SSN.LoadResources();
            this.EIN.LoadResources();
            this.CPF.LoadResources();
            this.PIS.LoadResources();
            this.MainPhone.LoadResources();
            this.MobilePhone.LoadResources();
            this.CommercialPhone.LoadResources();
            this.MessagesPhone.LoadResources();
            return this;
        }

        public virtual BasicInfoModel ApplyTo(Account account)
        {
            account.FirstName = this.FirstName;
            account.LastName = this.LastName;
            account.EmailAddress = this.Email;
            account.IsEntity = this.IsEntity;
            account.EntityName = this.IsEntity ? this.EntityName : string.Empty;
            account.DecryptedTaxNumber = this.TaxNumber;
            account.GenderID = this.Gender == ConstantsGenerated.Gender.NotSet ? null : (short?)this.Gender;
            if (this.Birthday != null)
            {
                account.Birthday = this.Birthday.Date;
            }

            account.MainPhone = this.MainPhone.Value.RemoveNonNumericCharacters();
            account.CellPhone = this.MobilePhone.Value.RemoveNonNumericCharacters();
            account.WorkPhone = this.CommercialPhone.Value.RemoveNonNumericCharacters();
            account.TextPhone = this.MessagesPhone.Value.RemoveNonNumericCharacters();
            return this;
        }

        public virtual BasicInfoModel ApplyTo(User user)
        {
            if (this.Password != _passwordPlaceholder)
            {
                user.Password = this.Password;
            }

            return this;
        }

        public virtual BasicInfoModel ApplyTo(Address address)
        {
            this.MainAddress.ApplyTo(address);

            return this;
        }
        #endregion

        #region validaciones CPF
        private static int Sum(int[] lista)
        {
            int total = 0;
            foreach (var num in lista)
            {
                total += num;
            }
            return total;
        }
        static bool ValidarCPF(string TextoInput)
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == 73)
            {
                if (TextoInput.Length < 11 || TextoInput == "")
                    return false;

                string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
                string PrimerDigito = TextoInput.Substring(9, 1);
                string SegundoDigito = TextoInput.Substring(10, 1);
                int PrimerDigitoValidar = ValidarPrimerDigito(NuevePrimerosDigitos);
                int SegundoDigitoValidar = ValidarSegundoDigito(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());
                return (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
            }
            return true;
        }
        static int ValidarPrimerDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[9];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Sum(Resultados);
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        static int ValidarSegundoDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Sum(Resultados);

            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        #region validaciones PIS
        static bool ValidarPIS(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

            return (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }

        static int ValidarSegundoDigitoPIS(string TextoValidar)
        {
            int[] Multiplicadores = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Sum(Resultados);
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion
    }
}