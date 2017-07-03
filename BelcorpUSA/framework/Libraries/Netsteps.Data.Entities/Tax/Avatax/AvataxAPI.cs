using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;

namespace NetSteps.Data.Entities.AvataxAPI
{
    /// <summary>
    /// Custom class to encapsulate all calls to the Avatax API
    /// </summary>
    public class AvataxAPI
    {
        TaxSvc taxSvc;

        #region properties
        private string _url;
        private string _viaUrl;
        private string _account;
        private string _key;
        private string _userName;
        private string _password;
        private string _companyCode;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public string ViaUrl
        {
            get { return _viaUrl; }
            set { _viaUrl = value; }
        }
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string CompanyCode
        {
            get { return _companyCode; }
            set { _companyCode = value; }
        }
        #endregion

        #region private methods
        private void LoadConfig()
        {
            taxSvc = new TaxSvc();
            taxSvc.Profile.Client = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_CLIENTPROFILE);

            Url = taxSvc.Configuration.Url = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_URL);
            //ViaUrl = taxSvc.Configuration.ViaUrl = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_VIAURL);
            //UserName = taxSvc.Configuration.Security.UserName = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_USERNAME);
            //Password = taxSvc.Configuration.Security.Password = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_PASSWORD);
            Account = taxSvc.Configuration.Security.Account = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ACCOUNT);
            Key = taxSvc.Configuration.Security.License = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_LICENSE);
            CompanyCode = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_COMPANYCODE);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// No parameter constructor
        /// </summary>
        public AvataxAPI()
        {
            LoadConfig();
        }
        public AvataxAPI(TaxSvc svc)
        {
            SetTaxSvc(svc);
        }

        public void SetTaxSvc(TaxSvc svc)
        {
            SetConfig(svc);
            CompanyCode = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_COMPANYCODE);
        }
        #endregion

        public void SetConfig(TaxSvc svc)
        {
            svc.Profile.Client = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_CLIENTPROFILE);

            if (Url != null)
            {
                svc.Configuration.Url = Url;
            }
            if (Account != null && Account.Length > 0)
            {
                svc.Configuration.Security.Account = Account;
            }
            if (Key != null && Key.Length > 0)
            {
                svc.Configuration.Security.License = Key;
            }
            svc.Configuration.Security.UserName = UserName;
            //write as plain text
            svc.Configuration.Security.Password = Password;

            taxSvc = svc;
        }

        public void SetConfig<T>()
        {
            BaseSvc svc = null;

            if (typeof(T).BaseType == typeof(BaseSvc))
                svc = new TaxSvc();

            svc.Profile.Client = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_CLIENTPROFILE);

            if (Url != null)
            {
                svc.Configuration.Url = Url;
            }
            if (Account != null && Account.Length > 0)
            {
                svc.Configuration.Security.Account = Account;
            }
            if (Key != null && Key.Length > 0)
            {
                svc.Configuration.Security.License = Key;
            }
            svc.Configuration.Security.UserName = UserName;
            //write as plain text
            svc.Configuration.Security.Password = Password;

            taxSvc = (TaxSvc)svc;
        }

        public PingResult PingAvatax()
        {
            return taxSvc.Ping("");
        }

        /// <summary>
        /// GetTax will call the API
        /// </summary>
        /// <param name="getTaxRequest">This parameter will be supplied by the adapter class</param>
        /// <returns>GetTaxResult: return value will be accepted by the adapter and in turn to Netsteps</returns>
        public GetTaxResult GetTax(GetTaxRequest getTaxRequest)
        {
            return taxSvc.GetTax(getTaxRequest);
        }
        /// <summary>
        /// PostTax will call the API for posting the tax
        /// </summary>
        /// <param name="getTaxRequest">This parameter will be supplied by the adapter class</param>
        /// <returns>GetTaxResult: return value will be accepted by the adapter and in turn to Netsteps</returns>
        public PostTaxResult PostTax(PostTaxRequest postTaxRequest)
        {
            return taxSvc.PostTax(postTaxRequest);
        }
        /// <summary>
        /// CommitTax will call the API for Committing the tax
        /// </summary>
        /// <param name="commitTaxRequest"></param>
        /// <returns></returns>
        public CommitTaxResult CommitTax(CommitTaxRequest commitTaxRequest)
        {
            return taxSvc.CommitTax(commitTaxRequest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commitTaxRequest"></param>
        /// <returns></returns>
        public CancelTaxResult CancelTax(CancelTaxRequest cancelTaxRequest)
        {
            return taxSvc.CancelTax(cancelTaxRequest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancelTaxRequest"></param>
        /// <returns></returns>
        public AdjustTaxResult AdjustTax(AdjustTaxRequest adjustTaxRequest)
        {
            return taxSvc.AdjustTax(adjustTaxRequest);
        }
    }
}
