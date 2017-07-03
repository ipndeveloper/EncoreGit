using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Testing.Integration.DWS.Support
{
    public abstract class DWS_Support_Base_Page : DWS_Base_Page
    {
        private DWS_Support_SectionNav_Control _secNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _secNav = _sectionNavList.As<DWS_Support_SectionNav_Control>();
        }

        public DWS_Support_SectionNav_Control SectionNav
        {
            get { return _secNav; }
        }
    }
}
