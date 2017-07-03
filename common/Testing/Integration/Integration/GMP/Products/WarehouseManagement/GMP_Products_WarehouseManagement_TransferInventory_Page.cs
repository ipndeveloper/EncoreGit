using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_TransferInventory_Page : GMP_Products_WarehouseManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("btnTransfer")).Exists;
        }
    }
}
