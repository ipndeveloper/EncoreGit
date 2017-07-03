﻿using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.CWS
{
    public class CWS_Home_Page : CWS_Base_Page
    {
        private Div _home;
        private Title _homeTitle;
        private ListItem _homeTab;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _home = _content.GetElement<Div>(new Param("HomeInner"));
            _homeTitle = Document.GetElement<Title>(new Param("Home", AttributeName.ID.InnerText, RegexOptions.None));
            _homeTab = Document.GetElement<ListItem>(new Param("current", AttributeName.ID.ClassName).And(new Param("Home", AttributeName.ID.InnerText)));
        }

        public override bool IsPageRendered()
        {
            return (_home.Exists || _homeTitle.Exists || _homeTab.Exists);
        }

        public string PageURL
        {
            get { return Util.Browser.Url; }
        } 
    }
}
