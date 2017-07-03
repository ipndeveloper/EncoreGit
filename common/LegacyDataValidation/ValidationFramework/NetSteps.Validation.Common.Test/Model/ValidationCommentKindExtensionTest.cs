using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common.Test.Model
{
    [TestClass]
    public class ValidationCommentKindExtensionTest
    {
        [TestMethod]
        public void ValidationCommentKindExtension_excludes_unincluded_commentkinds()
        {
            var kinds = ValidationCommentKind.Warning | ValidationCommentKind.Error;
            var specificKind = ValidationCommentKind.PrimaryRecordIdentifier;

            Assert.IsFalse(specificKind.IsIn(kinds));
        }

        [TestMethod]
        public void ValidationCommentKindExtension_includes_included_commentkinds()
        {
            var kinds = ValidationCommentKind.All;
            var specificKind = ValidationCommentKind.PrimaryRecordIdentifier;

            Assert.IsTrue(specificKind.IsIn(kinds));
        }

        [TestMethod]
        public void ValidationCommentKindExtension_exception_excludes_warning()
        {
            var kinds = ValidationCommentKind.Error;
            var specificKind = ValidationCommentKind.Warning;

            Assert.IsFalse(specificKind.IsIn(kinds));
        }

        [TestMethod]
        public void ValidationCommentKindExtension_warning_excludes_exception()
        {
            var kinds = ValidationCommentKind.Error;
            var specificKind = ValidationCommentKind.Warning;

            Assert.IsFalse(specificKind.IsIn(kinds));
        }
    }
}
