using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class UserNotificationTokenValueProvider : ITokenValueProvider
    {

        private const string USERNOTIFICATION_BEAUTYADVISOR = "UserNotificationBeautyAdvisor";
        private const string USERNOTIFICATION_ORDERNUMBER = "UserNotificationOrderNumber";
        private const string USERNOTIFICATION_SUBTOTAL = "UserNotificationSubTotal";
        private const string USERNOTIFICATION_TAX = "UserNotificationTax";
        private const string USERNOTIFICATION_SHIPPING = "UserNotificationShippingAndHandling";
        private const string USERNOTIFICATION_TOTAL = "UserNotificationTotal";

        private readonly string _beautyadvisor;
        private readonly string _ordernumber;
        private readonly string _subtotal;
        private readonly string _tax;
        private readonly string _shipping;
        private readonly string _total;

        public UserNotificationTokenValueProvider(string beautyadvisor, string ordernumber, string subtotal, string tax, string shipping, string total)
        {
            _beautyadvisor = beautyadvisor;
            _ordernumber = ordernumber;
            _subtotal = subtotal;
            _tax = tax;
            _shipping = shipping;
            _total = total;
        }

        public virtual IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                USERNOTIFICATION_BEAUTYADVISOR,
                USERNOTIFICATION_ORDERNUMBER,
                USERNOTIFICATION_SUBTOTAL,
                USERNOTIFICATION_TAX,
                USERNOTIFICATION_SHIPPING,
                USERNOTIFICATION_TOTAL
            };  
        }       
                
        public virtual string GetTokenValue(string token)
        {
            switch (token)
            {
                case USERNOTIFICATION_BEAUTYADVISOR:
                    return _beautyadvisor;
                case USERNOTIFICATION_ORDERNUMBER:
                    return _ordernumber;
                case USERNOTIFICATION_SUBTOTAL:
                    return _subtotal;
                case USERNOTIFICATION_TAX:
                    return _tax;
                case USERNOTIFICATION_SHIPPING:
                    return _shipping;
                case USERNOTIFICATION_TOTAL:
                    return _total;
                default:
                    return null;
            }
        }
    }
}
