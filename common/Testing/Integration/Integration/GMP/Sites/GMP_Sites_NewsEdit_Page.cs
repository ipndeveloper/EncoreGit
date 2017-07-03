﻿using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_NewsEdit_Page : GMP_Sites_WebsiteBase_Page
    {
        private CheckBox _public;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _public = Document.CheckBox("isPublic");
        }

         public override bool IsPageRendered()
        {
            return _public.Exists;
        }
    }
}