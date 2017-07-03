using System;
using System.Web.UI;

namespace nsCore.Areas.Reports
{
    public class OnPageLoad : Control
    {
        protected override void OnLoad(EventArgs e)
        {
            base.DataBind();
            if (this.Call != null)
                if (this.If == null || this.If(this.Page))
                    this.Call(this.Page);
        }

        public Predicate<Page> If { get; set; }
        public Action<Page> Call { get; set; }
    }
}