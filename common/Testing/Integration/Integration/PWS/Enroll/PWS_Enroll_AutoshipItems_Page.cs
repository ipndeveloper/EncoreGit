using WatiN.Core;
using System;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_AutoshipItems_Page : PWS_Base_Page
    {
        ElementCollection<Link> _products;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _products = _content.GetElements<Link>(new Param("FR Button MinorButton m5", AttributeName.ID.ClassName));
        }

        public override bool IsPageRendered()
        {
            throw new NotImplementedException();
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_AutoshipItems_Page SelectProduct(int? index = null, int? timeout = null)
        {
            Thread.Sleep(1000); // it seems we can click the add order but it is not really ready to be actioned.
            if (!index.HasValue)
                index = Util.GetRandom(0, _products.Count - 1);
            Util.LogDoneMessage(string.Format("PWS_Enroll_AutoshipItems_Page: Select index {0}", (int)index));
            _products[(int)index].CustomClick(timeout);
            return this;
        }
    }
}
