using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class OrderTokenValueProvider : ITokenValueProvider
    {
        #region ITokenValueProvider Members

        const string ORDER_NUMBER = "OrderNumber";
        const string DISTRIBUTOR_FULL_NAME = "DistributorFullName";
        const string ORDER_COMPLETE_DATE = "OrderDatePlaced";
        const string ORDER_TOTAL = "OrderTotal";

        private Order _order;

        public Order TokenOrder
        {
            get { return _order; }
            set { _order = value; }
        }

        public OrderTokenValueProvider(Order order)
        {
            _order = order;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>() {
                ORDER_NUMBER, 
                DISTRIBUTOR_FULL_NAME, 
                ORDER_COMPLETE_DATE, 
                ORDER_TOTAL 
            };
        }

        public string GetTokenValue(string token)
        {

            string tokenValue = null;

            switch (token)
            {
                case ORDER_NUMBER:
                    tokenValue = TokenOrder.OrderNumber;
                    break;
                case DISTRIBUTOR_FULL_NAME:
                    tokenValue = TokenOrder.Consultant.FullName;
                    break;
                case ORDER_COMPLETE_DATE:
                    tokenValue = TokenOrder.CompleteDate.ToShortDateStringDisplay(TokenOrder.Consultant.AccountCultureInfo);
                    break;
                case ORDER_TOTAL:
                    tokenValue = TokenOrder.GrandTotal.ToMoneyString(TokenOrder.Consultant.AccountCultureInfo);
                    break;
            }

            return tokenValue;
        }

        #endregion
    }
}
