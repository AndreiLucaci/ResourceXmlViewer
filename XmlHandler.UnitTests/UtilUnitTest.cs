using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlHandler.UnitTests
{
    [TestClass]
    public class UtilUnitTest
    {
        [TestMethod]
        public void Util_TestIFProgramIsInstalled_ReturnsTrueIfInstalledOtherWiseFalse()
        {
            Assert.IsTrue(Util.IsAppInstalled("Notepad++"));
        }
    }
}
