using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Generated;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Addresses.Common.Models;
using System.Web.Mvc;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class PaymentMethodsModel : SectionModel
    {
        /*CSTI(CS)-05/03/2016: Inicio*/
        public virtual CheckPaymentMethodsModel CheckPaymentMethods { get; set; }
        public virtual DirectDepositPaymentMethodsModel DirectDepositPaymentMethods { get; set; }

        public virtual MvcHtmlString MainAddressHtmlParent { get; set; }

        public BasicInfoModel BasicInfoModel { get; set; }

        /// <summary>
        /// Gets or sets AccountID.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Gets or sets EFTProfiles.
        /// </summary>
        public List<IEFTDisbursementProfile> DirectDepositProfiles { get; set; }

        /// <summary>
        /// Gets or sets CheckProfile.
        /// </summary>
        public ICheckDisbursementProfile CheckProfile { get; set; }

        /// <summary>
        /// Gets or sets CheckAddress.
        /// </summary>
        public Address CheckAddress { get; set; }

        /// <summary>
        /// Gets or set ChangeCountryURL
        /// </summary>
        public string ChangeCountryURL { get; set; }

        /// <summary>
        /// Gets or sets PostalCodeLookupURL.
        /// </summary>
        public string PostalCodeLookupURL { get; set; }

        /// <summary>
        /// Gets PaymentPreference.
        /// </summary>
        public DisbursementMethodKind PaymentPreference
        {
            get
            {
                var retVal = DirectDepositProfiles.Any(x => x.IsEnabled && x.IsEnabled)
                                                      ? DisbursementMethodKind.EFT
                                                      : (CheckProfile.IsEnabled && CheckProfile.IsEnabled ? DisbursementMethodKind.Check : DisbursementMethodKind.EFT);
                return retVal;
            }
        }

        public bool IsCheckPayment
        {
            get { return PaymentPreference == DisbursementMethodKind.Check; }
        }

        public bool IsDirectDepositPayment
        {
            get { return PaymentPreference == DisbursementMethodKind.EFT; }
        }

        /// <summary>
        /// Gets AddressOfRecord.
        /// </summary>
        public Address AddressOfRecord
        {
            get
            {
                if (Account.Addresses.Count > 0 && Account.Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).Count > 0)
                {
                    return Account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main);
                }

                return new Address();
            }
        }

        /// <summary>
        /// Loads the View Model 
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        public void CreateModel(Account account)
        {
            var service = Create.New<ICommissionsService>();

            Account = account;
            var profiles = service.GetDisbursementProfilesByAccountId(Account.AccountID);
            CheckProfile = profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.Check).Cast<ICheckDisbursementProfile>().FirstOrDefault() ?? Create.New<ICheckDisbursementProfile>();

            var addressId = CheckProfile.AddressId;
            CheckAddress = addressId > 0 && Address.Exists(addressId)
                                 ? Address.Load(addressId)
                                 : account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main)
                                      ?? new Address();

            CheckAddress.Attention = CheckProfile.NameOnCheck ?? (CheckProfile.NameOnAccount ?? CheckAddress.Attention);

            DirectDepositProfiles = profiles.Where(p => p.DisbursementMethod == DisbursementMethodKind.EFT).Cast<IEFTDisbursementProfile>().ToList();

            if (DirectDepositProfiles.Count >= 2)
            {
                return;
            }

            for (var i = DirectDepositProfiles.Count; i < 2; i++)
            {
                DirectDepositProfiles.Add(Create.New<IEFTDisbursementProfile>());
            }
        }

        #region Resources
        public virtual string TypePaymentMethods { get; set; }

        #endregion
        public PaymentMethodsModel()
        {
            this.CheckPaymentMethods = new CheckPaymentMethodsModel();
            this.DirectDepositPaymentMethods = new DirectDepositPaymentMethodsModel();
        }

        public virtual PaymentMethodsModel LoadValues(bool isSameShippingAddress, int countryID, IAddress mainAddress, IAddress shippingAddress)
        {
            //this.ShippingAddress.LoadValues(countryID);
            this.CheckPaymentMethods.LoadValues(isSameShippingAddress, countryID, mainAddress, shippingAddress);
            //this.CheckPaymentMethods.LoadResources(mainAddress);
            //this.DirectDepositPaymentMethods.lo
            return this;
        }

        public virtual PaymentMethodsModel LoadResources(IAddress mainAddress)
        {
            this.CheckPaymentMethods.LoadResources(mainAddress);
            //this.TypePaymentMethods = "Mensaje Prueba desde AcountInfo";
            return this;
        }
        /*CSTI(CS)-05/03/2016: Fin*/
    }
}