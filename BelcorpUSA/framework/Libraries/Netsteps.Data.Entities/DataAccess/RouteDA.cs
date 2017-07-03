using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public class RouteDA
    {
        public static List<RoutesData> Search()
        {
            List<RoutesData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRoutes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new RoutesData()
                        {
                            RouteID = Convert.ToInt32(reader["RouteID"]),
                            Name = Convert.ToString(reader["Name"])
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
    }
}
