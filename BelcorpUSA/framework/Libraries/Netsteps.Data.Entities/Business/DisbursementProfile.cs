using System.Collections.Generic;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class DisbursementProfile
    {
        private EFTProfile _eftProfile;
        /// <summary>
        /// This will return the EFTProfile associated with this DisbursementID - This is a 1:1 relationship
        /// </summary>
        public EFTProfile EFTProfile
        {
            get
            {
                if (_eftProfile == null && this.DisbursementTypeID == (int)NetSteps.Data.Entities.Constants.DisbursementProfileType.EFT)
                    _eftProfile = new EFTProfile(this);
                return _eftProfile;
            }
            set { _eftProfile = value; }
        }

        private CheckProfile _checkProfile;
        /// <summary>
        /// This will return the CheckProfile associated with this DisbursementID - This is a 1:1 relationship
        /// </summary>
        public CheckProfile CheckProfile
        {
            get
            {
                if (_checkProfile == null && this.DisbursementTypeID == (int)Constants.DisbursementProfileType.Check)
                    _checkProfile = new CheckProfile(this);
                return _checkProfile;
            }
            set { _checkProfile = value; }
        }

        private ProPayProfile _proPayProfile;
        /// <summary>
        /// This will return the ProPayProfile associated with this DisbursementID - This is a 1:1 relationship
        /// </summary>
        public ProPayProfile ProPayProfile
        {
            get
            {
                if (_proPayProfile == null && this.DisbursementTypeID == (int)NetSteps.Data.Entities.Constants.DisbursementProfileType.ProPay)
                    _proPayProfile = new ProPayProfile(this);
                return _proPayProfile;
            }
            set { _proPayProfile = value; }
        }

        #region Basic Crud
        public static List<DisbursementProfile> LoadByAccountID(int accountID)
        {
            var list = Repository.LoadByAccountID(accountID);
            foreach (var item in list)
            {
                item.StartEntityTracking();
                item.IsLazyLoadingEnabled = true;
            }
            return list;
        }

        public static string GetDisbursementTypeCode(int disbursementTypeId)
        {
            return Repository.GetDisbursementTypeCode(disbursementTypeId);
        }

        //public static void DeleteByAccountID(int accountID)
        //{
        //    Repository.DeleteByAccountID(accountID);
        //}

        public static void DisableByAccountID(int accountID)
        {
            Repository.DisableByAccountID(accountID);
        }

        public static List<DisbursementType> LoadEnabledDisbursementTypes()
        {
            return Repository.LoadEnabledDisbursementTypes();
        }
        #endregion
    }
}
