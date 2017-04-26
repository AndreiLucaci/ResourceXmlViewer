using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlHandler.XmlHandlerMocker;
using XmlHandler.XmlHandlerUtil;

namespace XmlHandler.UnitTests
{
    [TestClass]
    public class XmlUtilUnitTests
    {
        [TestMethod]
        public void XmlUtil_GetAllXElementItemsStartingWith_ReturnsListOfItemsStartingWith()
        {

            var _ = new XmlMocker();

            var sourceXml = _.CreateRootElement();
            _.Add("a.b.c.d", "asdf", sourceXml);
            _.Add("a.b.c.d2", "asdf", sourceXml);
            _.Add("a.b.c.e", "asdf", sourceXml);
            _.Add("a.b.c.d3", "asdf", sourceXml);

            var result = XmlUtil.GetAllXElementsStartingWith("a.b.c", sourceXml);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void XmlUtil_GetAllKeysStartingWith_ReturnsListOfItemsStartingWith()
        {

            var _ = new XmlMocker();

            var sourceXml = _.CreateRootElement();
            _.Add("a.b.c.d", "asdf", sourceXml);
            _.Add("a.b.c.d2", "asdf", sourceXml);
            _.Add("a.b.c.e", "asdf", sourceXml);
            _.Add("a.b.c.d3", "asdf", sourceXml);

            var result = XmlUtil.GetAllXElementsKeysStartingWith("a", sourceXml);

            Assert.AreEqual(1, result.Count);
        }
    }
}
