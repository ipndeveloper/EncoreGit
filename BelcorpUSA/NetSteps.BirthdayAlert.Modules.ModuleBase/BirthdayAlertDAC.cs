using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities;

namespace NetSteps.BirthdayAlert.Modules.ModuleBase
{
    public class BirthdayAlertDAC
    {
        public static List<Account> GetAccounts()
        {
            List<Account> entidades = new List<Account>();
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspGetAccountBirthdays"))
                {
                    
                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        Account entidad = new Account();
                        entidad.Id = DataConvertDA.ObjectToInt32(lector["Id"]);
                        entidad.BirthdayUTC = DataConvertDA.ObjectToDateTimeNull(lector["BirthdayUTC"]);
                        entidad.Email = DataConvertDA.ObjectToString(lector["Email"]);

                        entidades.Add(entidad);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            return entidades;
        }
        //fsv
    }
}
