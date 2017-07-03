﻿using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS
{
    /// <summary>
    /// Class related to PWS Home page controls and Ops.
    /// </summary>
    public class PWS_Home_Page : PWS_Base_Page
    {
        private Div _home, _slideNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _home = Document.GetElement<Div>(new Param("HomeInner"));
            _slideNav = _content.GetElement<Div>(new Param("slide-nav")); //Miche
        }

         public override bool IsPageRendered()
        {
            return (_home.Exists || _slideNav.Exists);
        }

        public string PageURL
        {
            get { return Util.Browser.Url; }
        } 
    }
}