namespace NetSteps.Data.Entities.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using NetSteps.Common.Base;
	using NetSteps.Common.Extensions;
	using NetSteps.Common.Globalization;
	using NetSteps.Data.Entities.Business.Interfaces;

	/// <summary>
    /// Author: John Egbert
    /// Description: Extensions to extend any SmallCollection classes
    /// Created: 06-09-2010
    /// </summary>
    public static class SmallCollectionExtensions
    {
        public static List<StateProvince> GetByCountryID(this Cache.SmallCollectionCache.StateProvinceCache list, int countryID)
        {
            return list.Where(s => s.CountryID == countryID).ToList();
        }

        public static BasicResponse IsActiveRule(this HostessRewardRule rule)
        {
            BasicResponse result = new BasicResponse() { Success = false };

            if (rule != null && rule.HostessRewardRuleID > 0)
            {
                if (DateTime.Now.IsBetween(rule.StartDate, rule.EndDate))
                {
                    result.Success = true;
                }
            }

            if (!result.Success)
            {
                result.Message = Translation.GetTerm("HostRewardRuleExpired", "Host Reward Rule Expired.");
            }

            return result;
        }

		public static T GetByCode<T>(this NetSteps.Data.Entities.Cache.SmallCollectionCache.SmallCollectionBase<T, Int32> list, string code) where T : ICode
        {
            return list.FirstOrDefault(i => i.Code == code);
        }

		public static T GetByIdOrNew<T, TKeyType>(this NetSteps.Data.Entities.Cache.SmallCollectionCache.SmallCollectionBase<T, TKeyType> list, TKeyType id)
			where T : new()
			where TKeyType : IComparable
		{
			T item = list.GetById(id);

			if (item == null || item.Equals(default(T)))
				item = new T();

			return item;
		}
    }
}
