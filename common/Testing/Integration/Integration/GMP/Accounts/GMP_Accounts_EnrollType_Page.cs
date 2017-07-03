using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollType_Page : GMP_Accounts_Base_Page
    {

        private RadioButton _rbtnRetailCustomer;
        private RadioButton _rbtnPreferredCustomer;
        private RadioButton _rbtnDistributor;
        private RadioButton _rbtnIndiDistributor;
        private RadioButton _rbtnBusEntDistributor;
        private Link _lnkNext;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this._rbtnRetailCustomer = Document.GetElement<RadioButton>(new Param("btnRetail"));
            this._rbtnPreferredCustomer = Document.GetElement<RadioButton>(new Param("btnPreferredCustomer"));
            this._rbtnDistributor = Document.GetElement<RadioButton>(new Param("btnDistributor"));
            this._rbtnIndiDistributor = Document.GetElement<RadioButton>(new Param("btnIndividual"));
            this._rbtnBusEntDistributor = Document.GetElement<RadioButton>(new Param("btnBusiness"));
            this._lnkNext = Document.GetElement<Link>(new Param("btnNext"));
        }

         public override bool IsPageRendered()
        {
            return _rbtnDistributor.Exists;
        }

         public GMP_Accounts_Enroll_Page EnrollRetailCustomer(int? timeout = null, bool pageRequired = true)
        {
            ChooseEnrollmentType(CustomerType.ID.Retail);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            page.ConfigurePage(CustomerType.ID.Retail);
            return page;
        }

         public GMP_Accounts_Enroll_Page EnrollPreferredCustomer(int? timeout = null, bool pageRequired = true)
        {
            ChooseEnrollmentType(CustomerType.ID.Preferred);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            page.ConfigurePage(CustomerType.ID.Preferred);
            return page;
        }


         public GMP_Accounts_Enroll_Page EnrollDistributor(int? timeout = null, bool pageRequired = true)
        {
            ChooseEnrollmentType(CustomerType.ID.Distributor);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            page.ConfigurePage(CustomerType.ID.Distributor);
            return page;
        }

         public GMP_Accounts_Enroll_Page EnrollBusinessEntity(int? timeout = null, bool pageRequired = true)
        {
            ChooseEnrollmentType(CustomerType.ID.Business);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            page.ConfigurePage(CustomerType.ID.Business);
            return page;
        }

        /// <summary>
        /// Choose enroller type.
        /// </summary>
        /// <param name="enrollmentType">Enroller type.</param>
        public GMP_Accounts_EnrollType_Page ChooseEnrollmentType(CustomerType.ID enrollmentType)
        {
            switch (enrollmentType)
            {
                case CustomerType.ID.Retail:
                    this._rbtnRetailCustomer.CustomSelectRadioButton();
                    break;

                case CustomerType.ID.Preferred:
                    this._rbtnPreferredCustomer.CustomSelectRadioButton();
                    break;

                case CustomerType.ID.Distributor:
                    _rbtnDistributor.CustomSelectRadioButton();
                    this._rbtnDistributor.CustomSelectRadioButton();
                    break;

                case CustomerType.ID.Business:
                    _rbtnDistributor.CustomSelectRadioButton();
                    this._rbtnBusEntDistributor.CustomSelectRadioButton();
                    break;
            }
            return this;
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            timeout = _lnkNext.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
