using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Linq.Expressions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;

//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Repositorio para los metodos utilizados con la entidad  BankPayments 

namespace NetSteps.Data.Entities.Repositories
{
    public class BankPaymentsRepository 
    {
        /// <summary>
        /// @1 Obtiene una lista con los registros de la tabla BankPayments
        /// </summary>
        /// <returns></returns>
        public  List<BankPayments> BrowseBankPayments()
        {
            List<BankPayments> result = new List<BankPayments>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetBankPayment", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<BankPayments>();
                    while (reader.Read())
                    {
                        result.Add(new BankPayments()
                        {
                            BankPaymentID = Convert.ToInt32(reader["BankPaymentID"]),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            BankName = Convert.ToString(reader["BankName"]),
                            DateReceivedBank = Convert.ToDateTime(reader["DateReceivedBank"]),
                            DateApplied = Convert.ToDateTime(reader["DateApplied"]),
                            TicketNumber = Convert.ToInt32(reader["TicketNumber"]),
                            OrderNumber = Convert.ToString(reader["OrderNumber"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            BankPaymentType = Convert.ToInt32(reader["BankPaymentType"]),
                            AccountCode = Convert.ToInt32(reader["AccountCode"]),
                            AccountName = Convert.ToString(reader["AccountName"]),
                            ResponsibleCode = GetValueOrNull<int>(reader["ResponsibleCode"].ToString()),
                            ResponsibleName = Convert.ToString(reader["ResponsibleName"]),
                            Applied = Convert.ToInt32(reader["Applied"].ToString()),
                            BankID = Convert.ToInt32(reader["BankID"]),
                            FileNameBank = Convert.ToString(reader["FileNameBank"]),
                            FileSequence = Convert.ToInt32(reader["FileSequence"]),
                            logSequence = Convert.ToInt32(reader["logSequence"]),
                            Credito = Convert.ToString(reader["Credito"]),
                            Residual = Convert.ToDecimal(reader["Residual"])
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
      
        public static T? GetValueOrNull<T>(string valueAsString)
           where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
    }
}
