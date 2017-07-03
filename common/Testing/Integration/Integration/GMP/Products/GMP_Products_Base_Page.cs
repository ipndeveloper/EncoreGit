using WatiN.Core;
using System;
using NetSteps.Testing.Integration.GMP.Products.ProductManagement;

namespace NetSteps.Testing.Integration.GMP.Products
{
    public abstract class GMP_Products_Base_Page : GMP_Base_Page
    {
        public GMP_Products_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Products_SubNav_Control>(); }
        }
    }
}
