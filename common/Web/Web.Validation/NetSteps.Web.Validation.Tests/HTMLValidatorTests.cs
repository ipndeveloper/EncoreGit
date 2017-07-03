using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Validation;

namespace NetSteps.Web.Validation.Tests
{
    [TestClass]
    public class HTMLValidatorTests
    {
        IHTMLValidator Validator
        {
            get
            {
                return Create.New<IHTMLValidator>();
            }
        }

        [TestMethod]
        public void CanValidateHtml()
        {
            string html = "<html><head><script></script></head><body><br /><a /></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AllowsCommentsInHtml()
        {
            string html = "<html><head></head><body><!-- this is the test comment --></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CanCatchInvalidHtml()
        {
            string html = "<html><ead></head><body></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

            foreach (var error in errors)
            {
                Assert.AreEqual("TagNotOpened", error.Code);
                Assert.AreEqual("Start tag <head> was not found", error.Reason);
            }
        }

        [TestMethod]
        public void CanCatchMissingClosingTag()
        {
            string html = "<html><head></head><body></body>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

            foreach (var error in errors)
            {
                Assert.AreEqual("TagNotClosed", error.Code);
                Assert.AreEqual("End tag </html> was not found", error.Reason);
            }
        }

        [TestMethod]
        public void CanCatchExtraClosingTag()
        {
            string html = "<div><ul></ul><table></table></div></div>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

            foreach (var error in errors)
            {
                Assert.AreEqual("TagNotOpened", error.Code);
                Assert.AreEqual("Start tag <div> was not found", error.Reason);
            }
        }

        [TestMethod]
        public void CanCatchBadTagOrdering()
        {
            string html = "<div><ul><table></ul></table></div>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

            foreach (var error in errors)
            {
                Assert.AreEqual("TagNotOpened", error.Code);
                Assert.AreEqual("Start tag <table> was not found", error.Reason);
            }
        }

        [TestMethod]
        public void CanCatchExtraParens()
        {
            string html = "<div><<ul></ul><table></table></div></div>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

            var errorList = errors.ToList();
            Assert.AreEqual("EndTagNotRequired", errorList[0].Code);
            Assert.AreEqual("End tag </> is not required", errorList[0].Reason);

            Assert.AreEqual("TagNotOpened", errorList[1].Code);
            Assert.AreEqual("Start tag <div> was not found", errorList[1].Reason);
        }

        [TestMethod]
        public void AllowsPlainText()
        {
            string html = "The quick brown fox";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AllowsAttributes()
        {
            string html = "<html name=\"theName\"><head></head><body></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AllowsCustomAttributes()
        {
            string html = "<html myAttribute=\"custom value\"><head></head><body></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CanAllowNonJQueryScripts()
        {
            string html = "<html><head><script type='text/javascript' src='some_url/my.js'></script></head><body></body></html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsTrue(isValid);

            foreach (var error in errors)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CanBlockJQueryScripts()
        {
            string html = @"
<html>
    <head>
        <script type='text/javascript' src='http://images.lbelusa.com/encore/js/camera/jquery.js'></script>
        <script type='text/javascript' src='http://code.jquery.com/jquery-1.9.1.min.js'></script>
        <script type='text/javascript' src='http://code.jquery.com/jquery-1.9.1.js'></script>
        <script type='text/javascript' src='http://code.jquery.com/jquery-1.10.1.min.js'></script>
        <script type='text/javascript' src='some_url/min-jquery.js'></script>
        <script type='text/javascript' src='some_url/jquery.min.js'></script>
		<script type='text/javascript' src='some_url/jquery.slides.min.js'></script> <!-- this line is ok, should not trigger -->
		<script type='text/javascript' src='some_url/jquery.calendar.js'></script> <!-- this line is ok, should not trigger -->
    </head>
    <body>
    </body>
</html>";
            IHTMLValidator validator = this.Validator;

            IEnumerable<IHTMLParseError> errors = null;
            bool isValid = validator.IsValid(html, out errors);
            Assert.IsFalse(isValid);

			int expectedErrorCount = 6;
            Assert.IsTrue(errors.Count() == expectedErrorCount, string.Format("error count was {0}, expected {1}", errors.Count(), expectedErrorCount));

            foreach (var error in errors)
            {
                Assert.IsTrue(error.Code == "INVALID_SCRIPT_INCLUDE");
            }
        }

    }
}
