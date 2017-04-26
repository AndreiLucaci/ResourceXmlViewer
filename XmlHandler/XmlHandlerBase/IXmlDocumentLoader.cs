using System.Xml.Linq;

namespace XmlHandler.XmlHandlerBase
{
    public interface IXmlDocumentLoader
    {
        XElement GetContents(string sourceFilePath);
    }
}