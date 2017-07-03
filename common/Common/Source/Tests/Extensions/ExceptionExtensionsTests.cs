using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Tests.Extensions
{
    [TestClass]
    public class ExceptionExtensionsTests
    {
        [TestMethod]
        public void GetRealException_Throws_ArgumentNullException()
        {
            var originalEx = null as Exception;
            Exception thrownEx = null;

            try
            {
                originalEx.GetRealException();
            }
            catch (Exception ex)
            {
                thrownEx = ex;
            }

            Assert.IsNotNull(thrownEx);
            Assert.IsInstanceOfType(thrownEx, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void GetRealException_Returns_InnerException()
        {
            string outerMessage = "Outer";
            string innerMessage = "Inner";
            var originalEx = new Exception(outerMessage, new Exception(innerMessage));

            var realEx = originalEx.GetRealException();

            Assert.AreEqual(innerMessage, realEx.Message);
        }

        [TestMethod]
        public void GetRealException_Stops_At_MaxLevels()
        {
            int maxLevels = 3;
            string message1 = "1", message2 = "2", message3 = "3", message4 = "4", message5 = "5";
            var originalEx = new Exception(
                message1, new Exception(
                    message2, new Exception(
                        message3, new Exception(
                            message4, new Exception(
                                message5)
                            )
                        )
                    )
                );

            var realEx = originalEx.GetRealException(maxLevels);

            Assert.AreEqual(message4, realEx.Message);
        }

        [TestMethod]
        public void GetRealException_Stops_BeforeEmpty()
        {
            string message1 = "1", message2 = "message after 2 is empty", message3 = "", message4 = "4", message5 = "5";
            var originalEx = new Exception(
                message1, new Exception(
                    message2, new Exception(
                        message3, new Exception(
                            message4, new Exception(
                                message5)
                            )
                        )
                    )
                );

            var realEx = originalEx.GetRealException(false, 0);

            Assert.AreEqual(message2, realEx.Message);
        }

        [TestMethod]
        public void GetRealException_CanGetDeapestNestedException()
        {
            string message1 = "1", message2 = "2", message3 = "3", message4 = "4", message5 = "5";
            var originalEx = new Exception(
                message1, new Exception(
                    message2, new Exception(
                        message3, new Exception(
                            message4, new Exception(
                                message5)
                            )
                        )
                    )
                );

            var realEx = originalEx.GetRealException(false, 0);

            Assert.AreEqual(message5, realEx.Message);
        }
    }
}
