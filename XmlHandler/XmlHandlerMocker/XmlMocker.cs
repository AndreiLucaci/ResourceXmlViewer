using System.Xml.Linq;
using XmlHandler.XmlHandlerUtil;

namespace XmlHandler.XmlHandlerMocker
{
    public class XmlMocker
    {
        private XElement _root;

        public XElement Root
        {
            get { return _root ?? (_root = XmlUtil.CreateRootXElement()); }
            set { _root = value; }
        }

        public XElement Add(string path, object content)
        {
            var xelem = XmlUtil.AddXElement(path, content, Root);
            return xelem;
        }

        public XElement CreateRootElement()
        {
            return XmlUtil.CreateRootXElement();
        }


        public XElement Add(string path, string content)
        {
            var xelem = XmlUtil.AddXElement(path, XmlUtil.CreateEmptyXElementLeaf(content), Root);
            return xelem;
        }

        public XElement Add(string path, string content, XElement root)
        {
            var xelem = XmlUtil.AddXElement(path, XmlUtil.CreateEmptyXElementLeaf(content), root);
            return xelem;
        }
    }
}
