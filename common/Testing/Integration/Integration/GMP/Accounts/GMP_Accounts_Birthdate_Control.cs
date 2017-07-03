using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Birthdate_Control : Control<TableCell>
    {
        private TextField _txtDOBMOnth;
        private TextField _txtDOBDay;
        private TextField _txtDOBYear;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtDOBMOnth = Element.GetElement<TextField>(new Param("txtDOBMonth"));
            _txtDOBDay = Element.GetElement<TextField>(new Param("txtDOBDay"));
            _txtDOBYear = Element.GetElement<TextField>(new Param("txtDOBYear"));
        }

        public void EnterBirthDate(DateTime birthdate)
        {
            _txtDOBYear.CustomWaitForExist();
            _txtDOBMOnth.CustomSetTextQuicklyHelper(birthdate.Month.ToString());
            _txtDOBDay.CustomSetTextQuicklyHelper(birthdate.Day.ToString());
            _txtDOBYear.CustomSetTextQuicklyHelper(birthdate.Year.ToString());
        }
    }
}
