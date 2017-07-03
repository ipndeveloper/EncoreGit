using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class MaterialsBusinessLogic
    {
        public IEnumerable<dynamic> GetAllMaterials()
        {
            var table = new Materials();
            var lista = table.All();
            return lista;
        }

        public IEnumerable<dynamic> GetAllMaterialsByParentProductID(int ParentProductID)
        {
            var table = new Materials();
            var lista = table.Query("EXEC uspMaterialsByParentProductID @0", new object[] { ParentProductID });
            return lista;
        }

        public IEnumerable<dynamic> GetReplacementsByProductRelationID(int ProductRelationID)
        {
            var table = new Materials();
            var lista = table.Query("EXEC uspProductRelationReplacementsByProductRelationID @0", new object[] { ProductRelationID });
            return lista;
        }

        public IEnumerable<dynamic> GetProductRelationsPerPeriod(int ProductRelationID)
        {
            var table = new Materials();
            var lista = table.Query("EXEC uspProductRelationsPerPeriodByProductRelationID @0", new object[] { ProductRelationID });
            return lista;
        }

        public IEnumerable<dynamic> GetReplacementsByProductRelationPerPeriod(int ProductRelationID)
        {
            var table = new Materials();
            var lista = table.Query("EXEC GetProductRelationPerPeriod @0", new object[] { ProductRelationID });
            return lista;
        }
    }
}
