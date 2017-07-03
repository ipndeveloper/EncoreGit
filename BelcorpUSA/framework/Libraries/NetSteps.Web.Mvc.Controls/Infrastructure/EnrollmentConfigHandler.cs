using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.XML;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Controls.Controllers.Enrollment;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;

namespace NetSteps.Web.Mvc.Controls.Infrastructure
{
    public class EnrollmentConfigHandler
    {
        private static dynamic _config = null;
        private static List<Type> _allTypes = null;
        protected readonly static object _lock = new object();

        public static dynamic Config
        {
            get { return _config; }
        }

        #region Events
        public static event EventHandler ConfigUpdated;
        protected static void OnConfigUpdated(object sender, EventArgs e)
        {
            if (ConfigUpdated != null)
                ConfigUpdated(sender, e);
        }
        #endregion

        public static Assembly _webSiteAssembly = null;
        public static Assembly WebSiteAssembly
        {
            get
            {
                return _webSiteAssembly;
            }
            set
            {
                if (_webSiteAssembly != value)
                {
                    _webSiteAssembly = value;

                    // The enrollment steps in the override assembly (if one is specified in the config)
                    string overrideAssemblyName = _config.Assemblies().Assembly().Name;
                    var overrideTypes = string.IsNullOrEmpty(overrideAssemblyName)
                        ? Enumerable.Empty<Type>()
                        : Assembly.Load(overrideAssemblyName).GetEnrollmentTypes();

                    // The enrollment steps in the website
                    var websiteTypes = _webSiteAssembly.GetEnrollmentTypes();

                    // The enrollment steps in NetSteps.Web.Mvc.Controls
                    var commonTypes = Assembly.GetExecutingAssembly().GetEnrollmentTypes();

                    // Union all steps by name (order is important: override -> website -> common)
                    _allTypes = overrideTypes
                        .UnionBy(websiteTypes, t => t.Name)
                        .UnionBy(commonTypes, t => t.Name)
                        .ToList();
                }
            }
        }

        public static List<Type> AllTypes
        {
            get { return _allTypes; }
        }

        static EnrollmentConfigHandler()
        {
            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "Enrollment.xml";
            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                _config = new DynamicXElement(XElement.Parse(new StreamReader(stream).ReadToEnd()));
                stream.Close();

                FileSystemWatcher watcher = new FileSystemWatcher(HttpContext.Current.Request.PhysicalApplicationPath);
                watcher.Filter = "Enrollment.xml";
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                watcher.Changed += new FileSystemEventHandler((sender, e) =>
                {
                    lock (_lock)
                    {
                        stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        _config = new DynamicXElement(XElement.Parse(new StreamReader(stream).ReadToEnd()));
                        stream.Close();
                        OnConfigUpdated(null, null);
                    }
                });

                watcher.EnableRaisingEvents = true;
            }

            _allTypes = Assembly.GetExecutingAssembly().GetEnrollmentTypes().ToList();
        }

        public static bool AccountTypeEnabled(short accountTypeID)
        {
            return bool.Parse(GetDynamicAccountTypeConfig(accountTypeID).Enabled);
        }

        public static IEnumerable<dynamic> GetProperties(EnrollmentStep currentStep)
        {
            string controlName = Regex.Replace(currentStep.GetType().Name, "Step$", "");

            return GetProperties(controlName);
        }

        public static IEnumerable<dynamic> GetProperties(string controlName)
        {
            dynamic control = _config.Controls().XPath(string.Format(".//Control[@Name='{0}']", controlName));
            if (control is DynamicXElement)
            {
                return control.Properties().Property();
            }

            return Enumerable.Empty<dynamic>();
        }

        public static dynamic GetProperty(string controlName, string propertyName)
        {
            var properties = GetProperties(controlName).ToList();
            return properties
                .FirstOrDefault(x => x.Name == propertyName);
        }

        public static List<short> GetEnabledAccountTypeIDs()
        {
            // WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
            IEnumerable<dynamic> accountTypesEnumerable = _config
                .AccountTypes()
                .AccountType();

            return accountTypesEnumerable
                .Where(x => bool.Parse((string)x.Enabled))
                .Select(x => short.Parse((string)x.AccountTypeID))
                .ToList();
        }

        public static EnrollmentConfig GetEnrollmentConfig(short accountTypeID)
        {
            var accountTypeConfig = GetDynamicAccountTypeConfig(accountTypeID);
            var config = new EnrollmentConfig();

            config.SiteTypeID = ApplicationContext.Instance.SiteTypeID;
            config.AccountTypeID = accountTypeID;
            config.Enabled = bool.Parse((string)accountTypeConfig.Enabled);
            config.Steps = new OrderedList<EnrollmentStepConfig>(
                GetDynamicStepConfigs(accountTypeID)
                    .Select(x => new EnrollmentStepConfig
                    {
                        Name = x.Name,
                        TermName = x.TermName,
                        Controller = x.UseControl,
                        Skippable = x.Skippable == "" ? false : Convert.ToBoolean(x.Skippable),
                        Sections = new OrderedList<EnrollmentStepSectionConfig>(
                            GetDynamicStepSectionConfigs(x as object, accountTypeID)
                                .Select(s => new EnrollmentStepSectionConfig
                                {
                                    Name = s.Name,
                                    TermName = s.TermName,
                                    Action = s.Action,
                                    Skippable = s.Skippable == "" ? false : Convert.ToBoolean(s.Skippable)
                                }
                            )
                        )
                    }
                )
            );
            config.Sponsor = GetSponsorConfig(accountTypeID);
            config.BasicInfo = GetBasicInfoConfig(accountTypeID);
            config.Billing = GetBillingConfig(accountTypeID);
            config.Website = GetWebsiteConfig();
            config.EnrollmentOrder = GetEnrollmentOrderConfig(accountTypeID);
            config.Autoship = GetAutoshipConfig(accountTypeID);
            config.Subscription = GetSubscriptionAutoshipConfig(accountTypeID);
            config.DisbursementProfiles = GetDisbursementConfig(accountTypeID);

            return config;
        }

        public static dynamic GetDynamicAccountTypeConfig(short accountTypeID)
        {
            var accountTypeDynamic = _config
                .AccountTypes()
                .XPath(string.Format(".//AccountType[@AccountTypeID='{0}']", accountTypeID));

            try
            {
                if (accountTypeDynamic.AccountTypeID == "")
                    throw new Exception();
            }
            catch
            {
                throw new Exception(string.Format("No enrollment config found for account type ID '{0}'", accountTypeID));
            }

            return accountTypeDynamic;
        }

        public static IEnumerable<dynamic> GetDynamicStepConfigs(short accountTypeID)
        {
            // WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
            IEnumerable<dynamic> stepConfigsEnumerable = GetDynamicAccountTypeConfig(accountTypeID)
                .Steps()
                .Step();

            if (!stepConfigsEnumerable.Any())
            {
                throw new Exception(string.Format("No enrollment steps found for account type ID '{0}'", accountTypeID));
            }

            return stepConfigsEnumerable;
        }

        public static IEnumerable<dynamic> GetDynamicStepSectionConfigs(dynamic stepConfig, short accountTypeID)
        {
            var sectionsProperty = GetProperty(stepConfig.UseControl, "Sections");
            if (sectionsProperty == null)
            {
                return Enumerable.Empty<dynamic>();
            }

            // WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
            IEnumerable<dynamic> sectionConfigsEnumerable = sectionsProperty
                .XPath(string.Format(".//Sections[@AccountTypeID='{0}']", accountTypeID))
                .Section();

            return sectionConfigsEnumerable;
        }

        public static EnrollmentSponsorConfig GetSponsorConfig(short accountTypeID)
        {
            var config = new EnrollmentSponsorConfig();

            dynamic property = GetProperty("Sponsor", "Sponsor");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//Sponsor[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.DenySponsorChange != "")
                        config.DenySponsorChange = Convert.ToBoolean(dynamicConfig.DenySponsorChange);
                }
            }

            return config;
        }

        public static EnrollmentBasicInfoConfig GetBasicInfoConfig(short accountTypeID)
        {
            var config = new EnrollmentBasicInfoConfig();

            dynamic property = GetProperty("AccountInfo", "BasicInfo");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//BasicInfo[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.SetShippingAddressFromMain != "")
                        config.SetShippingAddressFromMain = Convert.ToBoolean(dynamicConfig.SetShippingAddressFromMain);
                    if (dynamicConfig.SetBillingAddressFromMain != "")
                        config.SetBillingAddressFromMain = Convert.ToBoolean(dynamicConfig.SetBillingAddressFromMain);
                }
            }

            return config;
        }

        public static EnrollmentBillingConfig GetBillingConfig(short accountTypeID)
        {
            var config = new EnrollmentBillingConfig();

            dynamic property = EnrollmentConfigHandler.GetProperty("AccountInfo", "Billing");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//Billing[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.HideBillingAddress != "")
                        config.HideBillingAddress = Convert.ToBoolean(dynamicConfig.HideBillingAddress);
                }
            }

            return config;
        }

        public static EnrollmentWebsiteConfig GetWebsiteConfig()
        {
            var config = new EnrollmentWebsiteConfig();

            dynamic property = GetProperty("AccountInfo", "Website");
            if (property != null)
            {
                if (property.AutoshipScheduleID != "")
                    config.AutoshipScheduleID = Convert.ToInt32(property.AutoshipScheduleID);
            }

            return config;
        }

        public static EnrollmentOrderConfig GetEnrollmentOrderConfig(short accountTypeID)
        {
            var config = new EnrollmentOrderConfig();

            dynamic property = EnrollmentConfigHandler.GetProperty("Products", "InitialOrder");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//InitialOrder[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.ImportShoppingOrder != "")
                        config.ImportShoppingOrder = Convert.ToBoolean(dynamicConfig.ImportShoppingOrder);
                    if (dynamicConfig.SaveAsAutoshipOrder != "")
                        config.SaveAsAutoshipOrder = Convert.ToBoolean(dynamicConfig.SaveAsAutoshipOrder);
                    if (dynamicConfig.MinimumCommissionableTotal != "")
                        config.MinimumCommissionableTotal = Convert.ToDecimal(dynamicConfig.MinimumCommissionableTotal);
                }
            }

            return config;
        }

        public static EnrollmentAutoshipConfig GetAutoshipConfig(short accountTypeID)
        {
            var config = new EnrollmentAutoshipConfig();

            dynamic property = EnrollmentConfigHandler.GetProperty("Products", "Autoship");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//Autoship[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.AutoshipScheduleID != "")
                        config.AutoshipScheduleID = Convert.ToInt32(dynamicConfig.AutoshipScheduleID);
                    if (dynamicConfig.ImportShoppingOrder != "")
                        config.ImportShoppingOrder = Convert.ToBoolean(dynamicConfig.ImportShoppingOrder);
                    if (dynamicConfig.Hidden != "")
                        config.Hidden = Convert.ToBoolean(dynamicConfig.Hidden);
                    if (dynamicConfig.MinimumCommissionableTotal != "")
                        config.MinimumCommissionableTotal = Convert.ToDecimal(dynamicConfig.MinimumCommissionableTotal);
                    if (dynamicConfig.Skippable != "")
                        config.Skippable = Convert.ToBoolean(dynamicConfig.Skippable);
                }
            }

            return config;
        }

        public static EnrollmentSubscriptionAutoshipConfig GetSubscriptionAutoshipConfig(short accountTypeID)
        {
            var config = new EnrollmentSubscriptionAutoshipConfig();

            dynamic property = EnrollmentConfigHandler.GetProperty("Products", "Subscription");
            if (property != null)
            {
                dynamic dynamicConfig = property.XPath(string.Format(".//Subscription[@AccountTypeID='{0}']", accountTypeID));
                if (dynamicConfig is DynamicXElement)
                {
                    if (dynamicConfig.AutoshipScheduleID != "")
                        config.AutoshipScheduleID = Convert.ToInt32(dynamicConfig.AutoshipScheduleID);
                }
            }

            return config;
        }

        public static DisbursementProfilesConfig GetDisbursementConfig(short accountTypeID)
        {
            var config = new DisbursementProfilesConfig();

            IEnumerable<dynamic> disbProfilesProperties = EnrollmentConfigHandler.GetProperties("DisbursementProfiles");
            if (disbProfilesProperties != null)
            {
                var value = (string)disbProfilesProperties.FirstOrDefault(p => p.Name == "Hidden");
                if (!string.IsNullOrEmpty(value))
                {
                    config.Hidden = bool.Parse(value);
                }
                else
                {
                    config.Hidden = true;
                }
            }
            else
            {
                config.Hidden = true;
            }

            return config;
        }
    }
}