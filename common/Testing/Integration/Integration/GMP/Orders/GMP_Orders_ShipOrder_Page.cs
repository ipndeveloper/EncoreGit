using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_ShipOrder_Page : GMP_Orders_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<SelectList>(new Param("OrderShipmentStatusID")).Exists;
        }
    }
}
