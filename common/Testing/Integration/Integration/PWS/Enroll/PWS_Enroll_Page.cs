﻿using System.Text.RegularExpressions;
using WatiN.Core;
//using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Page : PWS_Base_Page
    {
        private PWS_Enroll_Culture_Control _distributor, _perferedCustomer;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _distributor = _content.GetElement<Div>(new Param("newDistrib", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(0))).As<PWS_Enroll_Culture_Control>();
            _perferedCustomer = _content.GetElement<Div>(new Param("newDistrib", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(1))).As<PWS_Enroll_Culture_Control>();
        }

         public override bool IsPageRendered()
        {
            return _distributor.Element.Exists;
        }

        public PWS_Enroll_Culture_Control Enrollment1
        {
            get { return _distributor; }
        }

        public PWS_Enroll_Culture_Control Enrollment2
        {
            get { return _perferedCustomer; }
        }
    }
}