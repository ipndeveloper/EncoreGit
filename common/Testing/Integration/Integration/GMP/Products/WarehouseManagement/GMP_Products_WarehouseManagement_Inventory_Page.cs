using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_Inventory_Page : GMP_Products_WarehouseManagement_Base_Page
    {
        private Button _save;
        private Link _transferInventory;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Button>(new Param("btnSave"));
            _transferInventory = Document.GetElement<Link>(new Param("/Products/Warehouses/Transfer", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _transferInventory.Exists;
        }

         public GMP_Products_WarehouseManagement_TransferInventory_Page ClickTransferWarehouseInventory(int? timeout = null, bool pageRequired = true)
        {
            timeout = _transferInventory.CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_TransferInventory_Page>(timeout, pageRequired);
        }
    }
}
