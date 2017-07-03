using System;
using System.Web.Mvc;
using Avalara.AvaTax.Adapter;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.AvataxAPI;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Security;

namespace nsCore.Areas.Admin.Controllers
{
    public class ConfigurationController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult ShowConfig()
        {
            ViewData["Avatax_Url"] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_URL);
            ViewData["Avatax_Account"] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ACCOUNT);
            ViewData["Avatax_License"] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_LICENSE);
            return View();
        }

        public virtual void EncryptPaymentGateway()
        {
            //ConfigurationEncryption.ProtectSectionGroup("PaymentGateways");
            ConfigurationEncryption.ProtectSectionGroup("PaymentGateways", "NetStepsProtectedConfigurationProvider");
        }

        public virtual void DecryptPaymentGateway()
        {
            ConfigurationEncryption.UnProtectSectionGroup("PaymentGateways");
        }

        public virtual ActionResult PingAvataxUrl(string Url, string License, string Account)
        {
            //string Url = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_URL);
            //string Account = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ACCOUNT);
            //string License = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_LICENSE);

            IAvataxAdapter avataxAdapter = null;
            bool pingSuccess = true;
            string resultMessage = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(Url.Trim()) && !string.IsNullOrEmpty(Account.Trim()) && !string.IsNullOrEmpty(License.Trim()))
                {
                    avataxAdapter = Create.New<IAvataxAdapter>();

                    PingResult pingResult = avataxAdapter.Ping();
                    pingSuccess = avataxAdapter.PingSuccess(pingResult);
                    resultMessage = avataxAdapter.PingResultMessage(pingResult);
                    avataxAdapter.Dispose();
                }

                if (!pingSuccess)
                    return Json(new { result = false, message = resultMessage });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }

            return Json(new { result = true, message = "Success" });
        }
    }
}
