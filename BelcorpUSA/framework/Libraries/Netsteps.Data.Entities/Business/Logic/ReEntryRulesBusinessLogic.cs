using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NetSteps.Data.Entities.Business.Logic
{
    public class ReEntryRulesBusinessLogic
    { 
        /// <summary>
        /// Created: KTC
        /// </summary>
        /// <returns>IEnumerable<dynamic> ReEntryRules </returns>
        public static IEnumerable<dynamic> GetReEntryRules()
        {
            var table = new ReEntryRules();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetReEntryRules");
            return List;
        }

        /// <summary>
        /// Created: KTC
        /// </summary>
        /// <returns>Dictionary<int, string> ReentryRulesValuesByType </returns>
        public static Dictionary<int, string> GetReentryRulesValuesByType(string ReentryRuleTypeID)
        {
            var table = new ReEntryRules();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetReentryRulesValuesByType @0", new object[] { ReentryRuleTypeID });

            Dictionary<int, string> listFin = new Dictionary<int, string>();
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID, item.Value);
            }
            return listFin;
        }
        /// <summary>
        /// Created : KTC
        /// </summary>
        /// <param name="model"></param>
        /// <returns>dynamic : Identity ID </returns>
        public dynamic Insert(dynamic model)
        {
            var table = new ReEntryRules();
            try
            {
                return table.Insert(new
                {
                    ReEntryRuleTypeID = model.ReEntryRuleTypeID,
                    ReEntryRuleValueID = model.ReEntryRuleValueID,
                    Active = true
                });
               
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Created: KTC
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        public bool Update(dynamic model)
        {
            var table = new ReEntryRules();
            try
            {               
                    table.Update(new
                    {
                        ReEntryRuleTypeID = model.ReEntryRuleTypeID,
                        ReEntryRuleValueID = model.ReEntryRuleValueID, 
                        Active = true
                    },
                    model.ReEntryRuleID
                    );   
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
