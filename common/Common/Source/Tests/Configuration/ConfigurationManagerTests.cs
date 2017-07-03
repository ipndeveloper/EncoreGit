using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Tests.Configuration
{
    [TestClass]
    public class ConfigurationManagerTests
    {
        [TestMethod]
        public void GetAbsoluteUploadPath_Tests()
        {
            string basePath = @"\\192.168.2.39\WebSites\nsCore5FileUploads\";
            string result;

            result = ConfigurationManager.GetAbsoluteUploadPath();
            Assert.AreEqual(basePath, result);

            result = ConfigurationManager.GetAbsoluteUploadPath(null);
            Assert.AreEqual(basePath, result);

            result = ConfigurationManager.GetAbsoluteUploadPath(@"foo", null);
            Assert.AreEqual(basePath + @"foo\", result);

            result = ConfigurationManager.GetAbsoluteUploadPath(@"foo");
            Assert.AreEqual(basePath + @"foo\", result);

            result = ConfigurationManager.GetAbsoluteUploadPath(@"foo\");
            Assert.AreEqual(basePath + @"foo\", result);

            result = ConfigurationManager.GetAbsoluteUploadPath(@"foo", @"bar");
            Assert.AreEqual(basePath + @"foo\bar\", result);

            result = ConfigurationManager.GetAbsoluteUploadPath(@"foo\", @"bar\");
            Assert.AreEqual(basePath + @"foo\bar\", result);
        }

        [TestMethod]
        public void GetWebUploadPath_Tests()
        {
            string basePath = @"/FileUploads/";
            string result;

            result = ConfigurationManager.GetWebUploadPath();
            Assert.AreEqual(basePath, result);

            result = ConfigurationManager.GetWebUploadPath(null);
            Assert.AreEqual(basePath, result);

            result = ConfigurationManager.GetWebUploadPath(@"foo", null);
            Assert.AreEqual(basePath + @"foo/", result);

            result = ConfigurationManager.GetWebUploadPath(@"foo");
            Assert.AreEqual(basePath + @"foo/", result);

            result = ConfigurationManager.GetWebUploadPath(@"foo/");
            Assert.AreEqual(basePath + @"foo/", result);

            result = ConfigurationManager.GetWebUploadPath(@"foo", @"bar");
            Assert.AreEqual(basePath + @"foo/bar/", result);

            result = ConfigurationManager.GetWebUploadPath(@"foo/", @"bar/");
            Assert.AreEqual(basePath + @"foo/bar/", result);
        }

        [TestMethod]
        public void AddWebUploadPath_Tests()
        {
            string basePath = @"/FileUploads/";
            string filename = "myphoto.jpg";
            string result;

            string nullString = null;
            result = nullString.AddWebUploadPath();
            Assert.AreEqual(basePath, result);

            result = nullString.AddWebUploadPath(null);
            Assert.AreEqual(basePath, result);

            result = filename.AddWebUploadPath();
            Assert.AreEqual(basePath + filename, result);

            result = filename.AddWebUploadPath(null);
            Assert.AreEqual(basePath + filename, result);

            result = filename.AddWebUploadPath(@"foo", null);
            Assert.AreEqual(basePath + @"foo/" + filename, result);

            result = filename.AddWebUploadPath(@"foo");
            Assert.AreEqual(basePath + @"foo/" + filename, result);

            result = filename.AddWebUploadPath(@"foo/");
            Assert.AreEqual(basePath + @"foo/" + filename, result);

            result = filename.AddWebUploadPath(@"foo", @"bar");
            Assert.AreEqual(basePath + @"foo/bar/" + filename, result);

            result = filename.AddWebUploadPath(@"foo/", @"bar/");
            Assert.AreEqual(basePath + @"foo/bar/" + filename, result);
        }

        [TestMethod]
        public void RemoveWebUploadPath_Tests()
        {
            string basePath = @"/FileUploads/";
            string filename = "myphoto.jpg";
            string result;

            string nullString = null;
            result = nullString.RemoveWebUploadPath();
            Assert.IsNull(result);

            result = filename.RemoveWebUploadPath();
            Assert.AreEqual(filename, result);
            
            result = (basePath + filename).RemoveWebUploadPath();
            Assert.AreEqual(filename, result);

            result = (basePath + filename).RemoveWebUploadPath(null);
            Assert.AreEqual(filename, result);

            result = (basePath + @"foo/" + filename).RemoveWebUploadPath(@"foo", null);
            Assert.AreEqual(filename, result);

            result = (basePath + @"foo/" + filename).RemoveWebUploadPath(@"foo/");
            Assert.AreEqual(filename, result);

            result = (basePath + @"foo/bar/" + filename).RemoveWebUploadPath(@"foo", @"bar");
            Assert.AreEqual(filename, result);

            result = (basePath + @"foo/bar/" + filename).RemoveWebUploadPath(@"foo/", @"bar/");
            Assert.AreEqual(filename, result);
        }
    }
}
