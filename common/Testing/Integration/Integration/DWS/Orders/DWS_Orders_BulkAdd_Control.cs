using System.Threading;
using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_BulkAdd_Control : Control<Div>
    {
        private SelectList _filter;
        private Link _resetQty, _closeBulk;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _filter = Element.GetElement<SelectList>();
            if (Element.GetElement<Link>(new Param("#bulkAddModal .quantity", AttributeName.ID.Onclick, RegexOptions.None)).Exists)
                _resetQty = Element.GetElement<Link>(new Param("#bulkAddModal .quantity", AttributeName.ID.Onclick, RegexOptions.None));
            else
                _resetQty = Element.GetElement<Link>(new Param("resetQuantities", "data-bind", RegexOptions.None));
            if (Element.GetElement<Link>(new Param("jqmClose FL", AttributeName.ID.ClassName).And(new Param(1))).Exists)
                _closeBulk = Element.GetElement<Link>(new Param("jqmClose FL", AttributeName.ID.ClassName).And(new Param(1)));
            else
                _closeBulk = Element.GetElement<Link>(new Param("jqmClose FL", AttributeName.ID.ClassName));
        }

        public DWS_Orders_BulkAdd_Control SelectFilter(int index)
        {
            _filter.CustomSelectDropdownItem(index);
            return this;
        }

        public DWS_Orders_BulkAdd_Control ResetQuantites()
        {
            _resetQty.CustomClick();
            return this;
        }

        public DWS_Orders_BulkAdd_Control DisplayAll()
        {
            Element.GetElement<Link>(new Param("FR", AttributeName.ID.ClassName, RegexOptions.None)).CustomClick();
            Element.CustomWaitForSpinners();
            return this;
        }

        public OrderItem_Control GetOrderItem(int? index = null, int skuIndex = 0)
        {
            Table tbl =  Element.GetElement<Table>(new Param("bulkProductCatalog"));
            if(!index.HasValue)
                index = Util.GetRandom(0, tbl.OwnTableRows.Count - 1);
            return tbl.GetElements<TableRow>()[(int)index].As<OrderItem_Control>().Configure(skuIndex);
        }

        public DWS_Orders_BulkAdd_Control AddToOrder()
        {
            Element.GetElement<Link>(new Param("btnBulkAdd")).CustomClick();
            Element.CustomWaitForSpinners();
            return this;
        }

        public void Close()
        {
            _closeBulk.CustomClick();
            Thread.Sleep(1000); //wait for closure
        }
    }
}
