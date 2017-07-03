using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
    [Serializable]
    public class ProductMatrixDto
    {
        public string CUV { get; set; }
        public int MaterialID { get; set; }
        public string Descripcion { get; set; }
        public string Mensaje { get; set; }

        public static explicit operator ProductMatrix(ProductMatrixDto obj)
            
        {
            ProductMatrix result = new ProductMatrix()
            {
                CUV = obj.CUV,
                MaterialID = obj.MaterialID,
                Descripcion = obj.Descripcion,
                Mensaje = obj.Mensaje
            };
            return result;
        }
    }
}
