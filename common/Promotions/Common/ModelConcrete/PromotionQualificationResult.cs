using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Common.ModelConcrete
{
    [Serializable]
    public class PromotionQualificationResult
    {
        private PromotionQualificationResult()
        {
            MatchingOrderCustomerAccountIDs = new List<int>();
        }

        public MatchType Status { get; private set; }
        protected List<int> MatchingOrderCustomerAccountIDs { get; set; }

        public enum MatchType
        {
            NoMatch,
            MatchForAllCustomers,
            MatchForSpecificCustomers
        }

        public void BitwiseAnd(PromotionQualificationResult match)
        {
            if (match.Status == MatchType.NoMatch)
                Status = MatchType.NoMatch;
            switch (Status)
            {
                case MatchType.NoMatch:
                    break;
                case MatchType.MatchForAllCustomers:
					if (match.Status == MatchType.MatchForSpecificCustomers)
					{
						Status = MatchType.MatchForSpecificCustomers;
						MatchingOrderCustomerAccountIDs = match.MatchingOrderCustomerAccountIDs;
					}
					break;
                case MatchType.MatchForSpecificCustomers:
                    if (match.Status == MatchType.MatchForSpecificCustomers)
                    {
                        MatchingOrderCustomerAccountIDs = MatchingOrderCustomerAccountIDs.Intersect(match.MatchingOrderCustomerAccountIDs).ToList();
                        if (MatchingOrderCustomerAccountIDs.Count == 0)
                            Status = MatchType.NoMatch;
                    }
                    break;
            }
        }

        public static implicit operator bool(PromotionQualificationResult match)
        {
            switch (match.Status)
            {
                case MatchType.NoMatch:
                    return false;
                case MatchType.MatchForAllCustomers:
                    return true;
                case MatchType.MatchForSpecificCustomers:
                    return match.MatchingOrderCustomerAccountIDs.Count > 0;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static PromotionQualificationResult MatchForAllCustomers
        {
            get
            {
                return new PromotionQualificationResult() { Status = MatchType.MatchForAllCustomers };
            }
        }

        public static PromotionQualificationResult NoMatch
        {
            get
            {
                return new PromotionQualificationResult() { Status = MatchType.NoMatch };
            }
        }

		public static PromotionQualificationResult MatchForSelectCustomers(int OrderCustomerId)
		{
			return MatchForSelectCustomers(new int[] { OrderCustomerId });
		}

        public static PromotionQualificationResult MatchForSelectCustomers(IEnumerable<int> OrderCustomerIds)
        {
            var obj = new PromotionQualificationResult() { Status = MatchType.MatchForSpecificCustomers };
            obj.MatchingOrderCustomerAccountIDs.AddRange(OrderCustomerIds);
            return obj;
        }

        public bool MatchForCustomerAccountID(int AccountId)
        {
            switch (Status)
            {
                case MatchType.MatchForAllCustomers:
                    return true;
                case MatchType.MatchForSpecificCustomers:
                    return MatchingOrderCustomerAccountIDs.Contains(AccountId);
                case MatchType.NoMatch:
                    return false;
            }
            return false;
        }
    }
}
