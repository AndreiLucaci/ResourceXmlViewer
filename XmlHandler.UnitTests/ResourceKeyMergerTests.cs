using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using XmlHandler.XmlHandlerBase;
using XmlHandler.XmlHandlerMocker;
using XmlHandler.XmlHandlerUtil;

namespace XmlHandler.UnitTests
{
    [TestClass]
    public class ResourceKeyMergerTests
    {
        private readonly string SOURCE_DUMMY_PATH = "a/b/c";
        private readonly string TARGET_DUMMY_PATH = "a/b/c";

        [TestMethod]
        public void ResourceMergerKey_MergeKeys_AddAndDeleteKeys()
        {


        }


        [TestMethod]
        public void XmlUtils_GetXElementFromPath_ReturnsXElementFromThePathOrNull()
        {
            var sourceXml = XElement.Parse(
@"<resources>
<item key='content'>
    <item key='test'>
        <item key='asdf'>
            <val>asdf</val>
        </item>
    </item>
</item>
</resources>");
            var path = "content.test.asdf";
            var result = XmlUtil.GetXElement(path, sourceXml);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void XmlUtils_GetXElmentFromPathWithLineNumber_ReturnsLineNumber()
        {
            var sourceXml = XElement.Parse(
@"<resources>
<item key='content'>
    <item key='test'>
        <item key='asdf'>
            <val>asdf</val>
        </item>
    </item>
    <item key='asdf'>
        <item key='asdf2'>
            <item key='this'>
                <val>asdf</val>
            </item>
        </item>
    </item>
</item>
</resources>", LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            var path = "content.asdf.asdf2.this";
            var result = XmlUtil.GetXElement(path, sourceXml);
            var lineNr = ((IXmlLineInfo) result).LineNumber;

            Assert.AreEqual(10, lineNr);
        }

        [TestMethod]
        public void XmlUTils_AddNewNodeBasedOnPath_ReturnsTheNewNodeAdded()
        {
            var _ = new XmlMocker();
            _.Add("common.test.key1", "value1");
            _.Add("common.test.key2", "value2");
            _.Add("common.test.key3.asdf1", "value3");
            _.Add("common.test.key3.asdf4", "value4");
            _.Add("test.path1", "testpath");

            var result = _.Root;
            Assert.IsNotNull(result);
        }


        private ResourceKeyMerger CreateSut(XElement sourceXml, XElement targetXml)
        {
            var documentLoader = Mock.Of<IXmlDocumentLoader>(i => i.GetContents(SOURCE_DUMMY_PATH) == sourceXml 
                && i.GetContents(TARGET_DUMMY_PATH) == targetXml);
            var sut = new ResourceKeyMerger(documentLoader, SOURCE_DUMMY_PATH, TARGET_DUMMY_PATH);
            return sut;

        }
    }
}
