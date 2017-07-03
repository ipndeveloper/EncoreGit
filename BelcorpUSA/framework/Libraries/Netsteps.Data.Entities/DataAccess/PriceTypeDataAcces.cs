using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public class PriceTypeDataAcces
    {
        //KLC - CSTI
        public static int InsertProductPriceType(ProductPriceType priceTypes)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@ProductPriceTypeID", priceTypes.ProductPriceTypeID },
                                                                                            { "@Name", priceTypes.Name }, 
                                                                                            { "@TermName", priceTypes.TermName },
                                                                                            { "@Active", priceTypes.Active },
                                                                                            { "@Mandatory", priceTypes.Mandatory}};

                SqlCommand cmd = DataAccess.GetCommand("upsInsProductPriceTypes", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}
