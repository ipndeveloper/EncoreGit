namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using NetSteps.Addresses.Common.Models;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Generated;
    using nsDistributor.Areas.Enroll.Models.Shared;

    public class AdditionalInformationModel : SectionModel
    {
        //
        // GET: /Enroll/AdditionalInformationModel/


        #region Values
       //[NSDisplayName("Name", "Name")]
       // public virtual string Name { get; set; }  

        //[NSRequired]
        //[NSDisplayName("Relationship", "Relationship")]
        //public virtual string Relationship { get; set; }

        //[NSDisplayName("PhoneMainNumber", "Phone Number(Main)")]
        //[CustomValidation(typeof(BasicInfoModel), "ValidateMainPhone")]
        //public virtual PhoneModel MainPhone { get; set; }

        //[NSRequired]
        [NSDisplayName("SchoolineLevel", "Schooline Level")]
        public virtual string SchoolineLevel { get; set; }

        [NSRequired]
        [NSDisplayName("AuthNetworkData", "AuthNetworkData")]
        public virtual bool AuthNetworkData { get; set; }

        [NSRequired]
        [NSDisplayName("AuthEmailSend", "AuthEmailSend")]
        public virtual bool AuthEmailSend { get; set; }

        [NSRequired]
        [NSDisplayName("AuthShareData", "AuthShareData")]
        public virtual bool AuthShareData { get; set; }

        // Hidden
        public virtual int CountryID { get; set; }
        #endregion



        #region Models

        public virtual BasicAddressModel MainAddress { get; set; }

        public virtual List<AccountPropertyModel> AccountProperties { get; set; }

        #endregion



        protected const string _passwordPlaceholder = "~.FAKE.~";

        public AdditionalInformationModel()
        {
            this.MainAddress = new BasicAddressModel();

            //this.MainPhone = new PhoneModel();

            // Value type defaults (will be overwritten by LoadValues() and model binder)
            this.CountryID = (int)Constants.Country.Brazil;
        }

        /// <summary>
        /// Validation Rules
        /// </summary>


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




        public virtual AdditionalInformationModel LoadValues(
            int countryID,
            Account account,
            IAddress mainAddress,
            bool forcePasswordChange,
            bool showTaxNumber)
        {
            this.CountryID = countryID;
            //this.Name = account.FirstName;
            //this.Relationship = account.
            // If they already have a password, we use a placeholder


            //this.MainPhone.LoadValues(account.MainPhone.RemoveNonNumericCharacters(), this.CountryID);

            this.MainAddress.LoadValues(countryID, mainAddress);



            return this;
        }

        public virtual AdditionalInformationModel LoadResources()
        {

            //this.MainPhone.LoadResources();

            return this;
        }


    }

}