using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NetSteps.Auth.UI.Common.Enumerations;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Auth.UI.Common;

namespace DistributorBackOffice.Controllers
{
	
	public class IntPWSController : Controller
	{
        protected virtual Account GetAccountWithCredentails(string identifier, int credentialTypeID)
        {
            switch ((LoginCredentialTypes)credentialTypeID)
            {
                case LoginCredentialTypes.Username:
                case LoginCredentialTypes.CorporateUsername:
                    var userID = Create.New<IUserRepository>().GetByUsername(identifier).UserID;
                    return Account.LoadForSessionByUserID(userID);// Create.New<IAccountRepository>().LoadByUserIdFull(userID);
                case LoginCredentialTypes.Email:
                    return Account.LoadForSession(identifier, false);//.LoadNonProspectByEmailFull(identifier);
                case LoginCredentialTypes.AccountId:
                    return Account.LoadForSession(int.Parse(identifier));// Account.LoadFull(int.Parse(identifier));
                default:
                    throw new NetStepsException(Translation.GetTerm("Login_UnableToLocateAcount", "Unable to locate account"));
            }
        }

        [HttpPost]
        public virtual ActionResult ValidPWS(string username, string password)
        {
            try
            {
                var authResult = new miControlador().GetAuthUIService().Authenticate(username, password, new miControlador().CurrentSite.SiteID);
               // var authResult = new miControlador().GetAuthUIService().Authenticate(username, password, 2);

                var account = GetAccountWithCredentails(username, authResult.CredentialTypeID);

                var ret = false;
                //   BR-CD-003
                if (account != null
                   && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment
                   && account.EnrollmentDateUTC == null)
                {
                    var ordersPending = account.Orders.Select(x => x.OrderStatusID != Constants.OrderStatus.Pending.ToShort()).Count();

                    var otherOrders = account.Orders.ToList().FindAll(x => x.OrderStatusID == Constants.OrderStatus.Printed.ToShort()
                                                                       || (x.OrderStatusID == Constants.OrderStatus.Shipped.ToShort())
                                                                       || (x.OrderStatusID == Constants.OrderStatus.Paid.ToShort())).Count();


                    if (ordersPending == 0 && otherOrders == 0)
                    {                        
                        ret = true;
                    }
                    
                    if (ordersPending > 0 && otherOrders == 0)
                    {                        
                        ret = true;
                    }
                   
                }
                
                string Ruta="";
                if (ret == true)
                { 
                    string site = ConfigurationManager.AppSettings["SitePWS"];
                    Ruta = site;
                }

                string encrPassword = Encriptar(password);
                return Json(new { result = ret, ruta = Ruta, password = encrPassword }); 

            }
            catch (Exception ex)
            {
                return Json(new { result = false});
            }
        }

        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

		
	}

    class miControlador : BaseController
    {
        public  virtual IAuthenticationUIService GetAuthUIService()
		{
            return base.GetAuthUIService();
		}
    }
}
