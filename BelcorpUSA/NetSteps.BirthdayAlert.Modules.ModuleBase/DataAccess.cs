using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace NetSteps.BirthdayAlert.Modules.ModuleBase
{
    //class DataAccess
    //{
    //}

    internal sealed class ConfiguracionDAC
    {
        static SqlConnection conexion;
        static string cadenaConexion;

        static ConfiguracionDAC()
        {
            cadenaConexion = ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            conexion = new SqlConnection(cadenaConexion);
        }


        public static SqlConnection obtenerConexion()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
            return conexion;
        }

        public static SqlCommand obtenerComando(string nombreProcedimientoAlmacenado)
        {
            SqlCommand comando = new SqlCommand(nombreProcedimientoAlmacenado, obtenerConexion());
            comando.CommandType = CommandType.StoredProcedure;
            return comando;
        }

    }
}
