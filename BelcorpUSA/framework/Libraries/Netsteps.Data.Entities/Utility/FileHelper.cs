using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/*
 * @01 20150820 BR-E020 CSTI JMO: Added Save Method Overload
 */

namespace NetSteps.Data.Entities.Utility
{
    public static class FileHelper
    {
        /// <summary>
        /// Obtiene el texto de un archivo especificandole la ruta
        /// </summary>
        /// <param name="pathTemplate">Ruta del arhchivo</param>
        /// <returns></returns>
        public static string GetText(string pathTemplate)
        {
            string resul = "";
            using (StreamReader reader = new StreamReader(pathTemplate))
            {
                resul = reader.ReadToEnd();
            }
            return resul;
        }

        /// <summary>
        /// Graba un objeto memorystream en la ubicacion especificada
        /// </summary>
        /// <param name="ms">Data</param>
        public static void Save(MemoryStream ms, string uploadPath)
        {
            using (FileStream file = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }

        #region [@01 A01]

        /// <summary>
        /// Graba un objeto XML, basado en un parámetro string de entrada, en la ubicacion especificada
        /// </summary>
        /// <param name="stringXML">XML String</param>
        /// <param name="uploadPath">File Path</param>
        public static void Save(string stringXML, string uploadPath)
        {
            File.WriteAllText(uploadPath, stringXML);
        }

        #endregion
    }
}
