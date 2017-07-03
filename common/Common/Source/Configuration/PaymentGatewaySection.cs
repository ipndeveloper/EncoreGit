using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Web;

namespace NetSteps.Common.Configuration
{
    #region Configuration Sections

    public static class PaymentGatewaySections
    {
        private static Lazy<List<PaymentGatewaySection>> __singleton = new Lazy<List<PaymentGatewaySection>>(LoadConfigGroup, LazyThreadSafetyMode.ExecutionAndPublication);

        public static List<PaymentGatewaySection> Instance
        {
            get { return __singleton.Value; }
        }

        static List<PaymentGatewaySection> LoadConfigGroup()
        {
            List<PaymentGatewaySection> list = new List<PaymentGatewaySection>();

            ConfigurationSectionGroup sectionGroup = (ApplicationContextCommon.Instance.IsWebApp && HttpContext.Current != null)
                    ? System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(@"~\").GetSectionGroup("PaymentGateways") as ConfigurationSectionGroup
                    : System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("PaymentGateways") as ConfigurationSectionGroup;

            if (sectionGroup == null)
            {
                throw new Exception("Unable to find 'PaymentGateways' config section.");
            }

            foreach (PaymentGatewaySection section in sectionGroup.Sections)
                list.Add(section);

            return list;
        }
    }

    public class PaymentGatewaySection : ConfigurationSection
    {
        const string prop_Namespace = "Namespace";
        const string prop_Login = "Login";
        const string prop_Password = "Password";
        const string prop_MerchantAccountNumber = "MerchantAccountNumber";
        const string prop_Pin = "Pin";
        const string prop_PaymentGatewayID = "PaymentGatewayID";
        const string prop_TestUrl = "TestUrl";
        const string prop_LiveUrl = "LiveUrl";
        const string prop_TestLogin = "TestLogin";
        const string prop_TestPassword = "TestPassword";
        const string prop_TestMinAmount = "TestMinAmount";
        const string prop_TestFailAmount = "TestFailAmount";
        const string prop_TestFailCreditCardNumber = "TestFailCreditCardNumber";
        const string prop_TestTerminalId = "TestTerminalId";
        const string prop_LiveTerminalId = "LiveTerminalId";

        [ConfigurationProperty(prop_Namespace, IsRequired = true)]
        public string Namespace
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_Namespace], prop_Namespace); }
            set { this[prop_Namespace] = value; }
        }

        [ConfigurationProperty(prop_Login, IsRequired = true)]
        public string Login
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_Login], prop_Login); }
            set { this[prop_Login] = value; }
        }

        [ConfigurationProperty(prop_Password, IsRequired = true)]
        public string Password
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_Password], prop_Password); }
            set { this[prop_Password] = value; }
        }

        [ConfigurationProperty(prop_MerchantAccountNumber, IsRequired = true)]
        public string MerchantAccountNumber
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_MerchantAccountNumber], prop_MerchantAccountNumber); }
            set { this[prop_MerchantAccountNumber] = value; }
        }

        [ConfigurationProperty(prop_Pin, IsRequired = true)]
        public string Pin
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_Pin], prop_Pin); }
            set { this[prop_Pin] = value; }
        }

        [ConfigurationProperty(prop_PaymentGatewayID, IsRequired = false)]
        public int? PaymentGatewayID
        {
            get { return AppSettingHelper.GetAppSettingValue<int?>(this[prop_PaymentGatewayID], prop_PaymentGatewayID); }
            set { this[prop_PaymentGatewayID] = value; }
        }

        [ConfigurationProperty(prop_TestUrl, IsRequired = false)]
        public string TestUrl
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestUrl], prop_TestUrl); }
            set { this[prop_TestUrl] = value; }
        }

        [ConfigurationProperty(prop_LiveUrl, IsRequired = false)]
        public string LiveUrl
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_LiveUrl], prop_LiveUrl); }
            set { this[prop_LiveUrl] = value; }
        }

        [ConfigurationProperty(prop_TestLogin, IsRequired = false)]
        public string TestLogin
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestLogin], prop_TestLogin); }
            set { this[prop_TestLogin] = value; }
        }

        [ConfigurationProperty(prop_TestPassword, IsRequired = false)]
        public string TestPassword
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestPassword], prop_TestPassword); }
            set { this[prop_TestPassword] = value; }
        }

        [ConfigurationProperty(prop_TestMinAmount, IsRequired = false)]
        public string TestMinAmount
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestMinAmount], prop_TestMinAmount); }
            set { this[prop_TestMinAmount] = value; }
        }

        [ConfigurationProperty(prop_TestFailAmount, IsRequired = false)]
        public string TestFailAmount
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestFailAmount], prop_TestFailAmount); }
            set { this[prop_TestFailAmount] = value; }
        }

        [ConfigurationProperty(prop_TestFailCreditCardNumber, IsRequired = false)]
        public string TestFailCreditCardNumber
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestFailCreditCardNumber], prop_TestFailCreditCardNumber); }
            set { this[prop_TestFailCreditCardNumber] = value; }
        }

        [ConfigurationProperty(prop_TestTerminalId, IsRequired = false)]
        public string TestTerminalId
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_TestTerminalId], prop_TestTerminalId); }
            set { this[prop_TestTerminalId] = value; }
        }
        [ConfigurationProperty(prop_LiveTerminalId, IsRequired = false)]
        public string LiveTerminalId
        {
            get { return AppSettingHelper.GetAppSettingValue<string>(this[prop_LiveTerminalId], prop_LiveTerminalId); }
            set { this[prop_LiveTerminalId] = value; }
        }
        
    }

    #endregion

    internal static class AppSettingHelper
    {
        public static T GetAppSettingValue<T>(object result, string appKey)
        {
            if (result == null || string.IsNullOrEmpty(result.ToString()))
            {
                object obj = ConfigurationManager.AppSettings[appKey];

                if (obj == null)
                    return default(T);

                return (T)obj;
            }
            return (T)result;
        }
    }
}
