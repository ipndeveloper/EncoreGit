using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NetSteps.Data.Entities.EntityModels
{
    #region ProductPriceTypeOrder
    public class ProductPriceTypeOrder
    {
        public int ProductPriceTypeOrderID { get; set; }
        public int AccountID { get; set; }
        public int PreOrderID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public short OrderTypeID { get; set; }
        public short AccountTypeID { get; set; }
        public int ProductPriceTypeID { get; set; }
        public int PriceRelationshipTypeID { get; set; }
        public int CurrencyID { get; set; }
        public int StoreFrontID { get; set; }
        public decimal Price { get; set; }
    }
    
    public static class ManProductPriceTypeOrder
    {
        static List<ProductPriceTypeOrder> Lista = new List<ProductPriceTypeOrder>();

        public static void Add(ProductPriceTypeOrder entidad)
        {
            Lista.Add(entidad);
        }

        public static void Delete(ProductPriceTypeOrder entidad)
        {
            Lista.RemoveAll(donde => donde.AccountID == entidad.AccountID & donde.ProductID == entidad.ProductID);
        }

        public static void DeleteAccountID(ProductPriceTypeOrder entidad)
        {
            Lista.RemoveAll(donde => donde.AccountID == entidad.AccountID);
        }

        public static void DeleteAll()
        {
            Lista.Clear();
        }

        public static List<ProductPriceTypeOrder> Get()
        {
            return Lista;
        }
    }
    #endregion

    #region ProductPriceTypes
    public class ProductPriceTypes
    {
        public int ProductPriceTypeID { get; set; }
        public int ProductID { get; set; }
        public decimal Precio { get; set; }
    }

    public static class ManProductPriceType
    {
        static List<ProductPriceTypes> ListaProductPriceType = new List<ProductPriceTypes>();

        public static void Add(ProductPriceTypes entidad)
        {
            if (!Existe(entidad.ProductID, entidad.ProductPriceTypeID))
                ListaProductPriceType.Add(entidad);
        }

        public static bool Existe(int productID, int productPriceTypeID)
        {
            int numeroRegistro = ListaProductPriceType.Where(donde => donde.ProductID == productID & donde.ProductPriceTypeID == productPriceTypeID).Count();
            return (numeroRegistro > 0);
        }

        public static decimal ObtenerValor(int productID, int productPriceTypeID)
        {
            if (ListaProductPriceType.Where(donde => donde.ProductID == productID & donde.ProductPriceTypeID == productPriceTypeID).Count() == 0)
                return 0;
            decimal precio = ListaProductPriceType.Where(donde => donde.ProductID == productID & donde.ProductPriceTypeID == productPriceTypeID).ElementAt(0).Precio;
            return precio;
        }

        public static void Delete(ProductPriceTypes entidad)
        {
            ListaProductPriceType.RemoveAll(donde => donde.ProductID == entidad.ProductID);
        }

        public static void DeleteAll()
        {
            ListaProductPriceType.Clear();
        }

        public static List<ProductPriceTypes> Get()
        {
            return ListaProductPriceType;
        }
    }
    #endregion
}
