using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
namespace NetSteps.Data.Entities.Extensions
{
    public class UserExtensions
    {
        
        /// <summary>
        /// Create by FHP
        /// </summary>
        /// <returns>Select Id, Name</returns>
        public static Dictionary<string, string> GetUserTypes()
        {
            List<UtilSearchData.Select> wareHouseResult = DataAccess.ExecWithStoreProcedureLists<UtilSearchData.Select>("Core", "uspGetUserTypes").ToList();
            Dictionary<string, string> UserTypesResultDic = new Dictionary<string, string>();
            foreach (var item in wareHouseResult)
            {
                UserTypesResultDic.Add(Convert.ToString(item.Id), item.Name);
            }
            return UserTypesResultDic;
        }
    }
}
