using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XmlHandler.XmlHandlerBase
{
    public class ResourceKeyMerger
    {
        private readonly string _SourceFilePath;
        private readonly string _TargetFilePath;
        private readonly string _MergedFilePath;
        private readonly IXmlDocumentLoader _documentLoader;

        private readonly IEnumerable<int> _LangugageIdsToBeCopied;

        public ResourceKeyMerger(IXmlDocumentLoader documentLoader, string sourceFilePath, string targetFilePath)
        {
            if (documentLoader == null)
            {
                throw new ArgumentNullException("documentLoader");
            }
            _documentLoader = documentLoader;
            _SourceFilePath = sourceFilePath;
            _TargetFilePath = targetFilePath;
        }

        public XElement MergeResourceItems()
        {
            XElement _source = _documentLoader.GetContents(_SourceFilePath);
            XElement _target = _documentLoader.GetContents(_TargetFilePath);

            XElementAddItemsRecursively(_source, _target);
            XElementRemoveItemsRecursively(_source, _target);
            return _target;
        }

        private void XElementAddItemsRecursively(XElement source, XElement target)
        {
            if (XElementsAreKeyEqual(source, target) || (source.Name == "resources" && target.Name == "resources"))
            {
                foreach (var i in source.Elements())
                {
                    if (XElementIsLeafNode(i))
                        return;
                    if (XElementContainsChild(i, target))
                    {
                        var newTarget = XElementGetChildWithKey(target, i.Attribute("key").Value);
                        XElementAddItemsRecursively(i, newTarget);
                    }
                    else
                    {
                        var next = XElementGetNextCommonSibling(i, source, target);
                        if (next == null)
                        {
                            target.Add(i);
                        }
                        else
                        {
                            next.AddBeforeSelf(i);
                        }
                    }
                }
            }
        }

        private void XElementRemoveItemsRecursively(XElement source, XElement target)
        {
            if (XElementsAreKeyEqual(source, target) || (source.Name == "resources" && target.Name == "resources"))
            {
                foreach (var i in target.Elements())
                {
                    if (XElementIsLeafNode(i))
                        return;
                    if (XElementContainsChild(i, source))
                    {
                        var newSource = XElementGetChildWithKey(source, i.Attribute("key").Value);
                        XElementRemoveItemsRecursively(newSource, i);
                    }
                    else
                    {
                        i.Remove();
                    }
                }
            }
        }

        private bool XElementContainsChild(XElement searchItem, XElement target)
        {
            return target.Elements().Any(i => i.HasAttributes && i.Attribute("key") != null && i.Attribute("key").Value == searchItem.Attribute("key").Value);
        }

        private XElement XElementGetNextCommonSibling(XElement searchElement, XElement source, XElement target)
        {
            var afterNodes = source.Elements().SkipWhile(i => i.Attribute("key").Value != searchElement.Attribute("key").Value);
            var commonNode = afterNodes.FirstOrDefault(i => XElementContainsChild(i, target));
            return target.Elements().FirstOrDefault(i => commonNode != null && i.Attribute("key").Value == commonNode.Attribute("key").Value);
        }

        private XElement XElementGetChildWithKey(XElement parent, string key)
        {
            return parent.Elements().First(j => j.HasAttributes && j.Attribute("key") != null && j.Attribute("key").Value == key);
        }

        private bool XElementIsLeafNode(XElement element)
        {
            return element.Name == "val";
        }

        private bool XElementsAreKeyEqual(XElement source, XElement target)
        {
            if (source.Name != "item" || target.Name != "item")
            {
                return false;
            }
            var sourceAttr = XElementGetKeyAttribute(source);
            var targetAttr = XElementGetKeyAttribute(target);
            return sourceAttr != null && targetAttr != null && sourceAttr == targetAttr;
        }

        private string XElementGetKeyAttribute(XElement element)
        {
            return element.HasAttributes && element.Attribute("key") != null ? element.Attribute("key").Value : null;
        }
    }
}