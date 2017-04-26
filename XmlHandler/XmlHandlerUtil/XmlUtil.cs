using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace XmlHandler.XmlHandlerUtil
{
    public static class XmlUtil
    {
        public static XElement GetXElement(string path, XElement currentLevel)
        {
            if (currentLevel.Name == "val" || path == "")
                return null;

            var dotIndex = path.IndexOf(".");
            var currentLevelKey = path.Substring(0, dotIndex < 0 ? path.Length : dotIndex);
            foreach (var i in currentLevel.Elements())
            {
                if (i.Name == "val")
                    return null;
                if (!i.HasAttributes || i.Attribute("key") == null || i.Attribute("key").Value != currentLevelKey)
                    continue;
                if (dotIndex < 0)
                {
                    return i;
                }
                path = path.Substring(dotIndex).TrimStart('.');
                return GetXElement(path, i);
            }
            return null;
        }

        public static int GetLineNumberOfXElement(string path, XElement currentLevel)
        {
            var item = (IXmlLineInfo)GetXElement(path, currentLevel);
            return (item == null || !item.HasLineInfo()) ? -1 : item.LineNumber;
        }

        public static XElement AddXElement(string path, object value, XElement root)
        {
            var xelem = GetXElement(path, root);
            return xelem ?? CreateXElementRecursive(path, value, root);
        }

        private static XElement CreateXElementRecursive(string path, object value, XElement currentLevel)
        {
            if (currentLevel.Name == "val" || path == "")
                return null;

            var dotIndex = path.IndexOf(".");
            var currentLevelKey = path.Substring(0, dotIndex < 0 ? path.Length : dotIndex);

            if (!XElementContainsChild(currentLevelKey, currentLevel))
            {
                currentLevel.Add(CreateXElementEmpty(currentLevelKey));
            }

            foreach (var i in currentLevel.Elements())
            {
                if (i.Name == "val")
                    return null;
                if (i.HasAttributes && i.Attribute("key") != null && i.Attribute("key").Value == currentLevelKey)
                {
                    if (dotIndex < 0)
                    {
                        i.Add(value);
                        return i;
                    }
                    path = path.Replace(currentLevelKey, "").TrimStart('.');
                    return CreateXElementRecursive(path, value, i);
                }
            }
            return null;
        }

        public static List<XElement> GetAllXElementsStartingWith(string path, XElement currentLevel)
        {
            var lastDotIndex = path.LastIndexOf('.');
            var key = path;
            if (lastDotIndex > 0)
            {
                key = path.Substring(lastDotIndex).TrimStart('.');
                path = path.Substring(0, lastDotIndex);
                currentLevel = GetXElement(path, currentLevel);
            }
            return GetAllXElementsStartingWithFromCurrentLevel(key, currentLevel);
        }

        private static List<XElement> GetAllXElementsStartingWithFromCurrentLevel(string key, XElement currentLevel)
        {
            var list = new List<XElement>();
            if (!currentLevel.HasElements || (currentLevel.HasElements && currentLevel.Elements().First().Name == "val"))
                return list;

            list.AddRange(currentLevel.Elements().Where(i => i.Attribute("key").Value.StartsWith(key)));

            return list;
        }

        private static List<string> GetAllXElementsKeyStartingWithFromCurrentLevel(string key, XElement currentLevel)
        {
            var list = new List<string>();
            if (currentLevel == null || !currentLevel.HasElements || (currentLevel.HasElements && currentLevel.Elements().First().Name == "val"))
                return list;

            list.AddRange(key == "" 
                ? currentLevel.Elements().Select(i => i.Attribute("key").Value) 
                : currentLevel.Elements().Where(i => i.Attribute("key").Value.StartsWith(key)).Select(i => i.Attribute("key").Value));

            return list;
        }

        public static List<string> GetAllXElementsKeysStartingWith(string path, XElement currentLevel)
        {
            //path = string.IsNullOrEmpty(path) ? "resources" : path;
            var lastDotIndex = path.LastIndexOf('.');
            var key = path;
            if (lastDotIndex > 0)
            {
                key = path.Substring(lastDotIndex).TrimStart('.');
                path = path.Substring(0, lastDotIndex);
                currentLevel = GetXElement(path, currentLevel);
            }
            return new List<string>(GetAllXElementsKeyStartingWithFromCurrentLevel(key, currentLevel).Select(i => key == path ? i : string.Format("{0}.{1}", path, i)));
        } 

        public static void XElementAddItemsRecursively(XElement source, XElement target)
        {
            if (!XElementsAreKeyEqual(source, target) && (source.Name != "resources" || target.Name != "resources"))
                return;
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

        public static string GetStringFromFile(string path)
        {
            var sourceData = File.ReadAllBytes(path);
            return GetStringFromByteArray(sourceData);
        }

        public static string GetStringFromByteArray(byte[] sourceData)
        {
            var bom = (sourceData[0] == 239 && sourceData[1] == 187 && sourceData[2] == 191);
            var substract = bom ? 3 : 0;
            return Encoding.UTF8.GetString(sourceData, bom ? substract : 0, sourceData.Length - substract);
        }

        public static void XElementRemoveItemsRecursively(XElement source, XElement target)
        {
            if (!XElementsAreKeyEqual(source, target) && (source.Name != "resources" || target.Name != "resources"))
                return;
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

        public static string FormatXElementNoTags(XElement element)
        {
            return string.Format("{0}{1}", element.Name, GetAttributes(element));
        }

        public static string GenerateFormatedXElement(XElement element)
        {
            var str = CreateXmlUTF8Header();
            FormatXElement(element, ref str);
            return str;
        }

        public static string XmlHeaderTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        public static string CreateXmlUTF8Header()
        {
            return XmlHeaderTemplate + Environment.NewLine;
        }

        private static void FormatXElement(XElement element, ref string str, int tabCount = 0)
        {
            str += TabIndent(tabCount++) + FormatXElementNode(element);
            if (element.HasElements)
            {
                foreach (var i in element.Elements())
                {
                    if (i.Name == "val")
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(i.ToString());
                        str += TabIndent(tabCount) + Encoding.UTF8.GetString(bytes) + Environment.NewLine;
                    }
                    else
                    {
                        FormatXElement(i, ref str, tabCount++);
                        tabCount--;
                    }
                }
            }
            str += TabIndent(--tabCount) + FormatXElementEndNode(element);
        }

        public static string GenerateXElementPath(XElement element)
        {
            if (element == null) return "";
            var path = "";
            while (element != null && element.Parent != null && (element.HasAttributes && element.Attribute("key") != null))
            {
                path = string.Format("{0}.{1}", path, element.Attribute("key").Value);
                element = element.Parent;
            }
            return string.Join(".", path.Trim('.').Split('.').Reverse());
        }

        private static string TabIndent(int tabCount = 0)
        {
            return new string('\t', tabCount);
        }

        private static string FormatXElementNode(XElement element, bool addNewLine = true)
        {
            return string.Format("<{0}{1}>" + (addNewLine ? Environment.NewLine : ""), element.Name, GetAttributes(element));
        }

        private static string FormatXElementLeafNode(XElement element)
        {
            return string.Format("{0}{1}{2}", FormatXElementNode(element, false), element.Nodes().First(), FormatXElementEndNode(element));
        }

        private static object XElementLeafValue(XElement element)
        {
            var cdata = element.Nodes().OfType<XCData>().FirstOrDefault();
            return (object)cdata ?? element.Value;
        }

        public static StringBuilder FormatXElement(XElement element)
        {
            var str = new StringBuilder(CreateXmlUTF8Header());
            foreach (var line in Regex.Split(element.ToString(), Environment.NewLine).Where(i => !string.IsNullOrEmpty(i)))
            {
                str.AppendLine(ReplaceSpacesWithTabs(line));
            }
            return str;
        }

        private static string ReplaceSpacesWithTabs(string line)
        {
            var res = new string(line.TakeWhile(i => i == ' ').ToArray());
            return string.IsNullOrEmpty(res) ? line : line.Replace(res, new string('\t', res.Count() / 2));
        }

        private static string FormatXElementEndNode(XElement element, bool addNewLine = true)
        {
            return string.Format("</{0}>" + (addNewLine ? Environment.NewLine : ""), element.Name);
        }

        private static XElement CreateXElementEmpty(string key)
        {
            return new XElement("item", new XAttribute("key", key));
        }

        private static bool XElementContainsChild(string key, XElement target)
        {
            return target.Elements().Any(i => i.HasAttributes && i.Attribute("key") != null && i.Attribute("key").Value == key);
        }

        public static XElement CreateEmptyXElementLeaf(string content)
        {
            return XElement.Parse(string.Format(itemLeafTemplate, content));
        }

        public static XElement CreateRootXElement()
        {
            return new XElement("resources");
        }

        private static bool XElementContainsChild(XElement searchItem, XElement target)
        {
            return target.Elements().Any(i => i.HasAttributes && i.Attribute("key") != null && i.Attribute("key").Value == searchItem.Attribute("key").Value);
        }

        private static XElement XElementGetNextCommonSibling(XElement searchElement, XElement source, XElement target)
        {
            var afterNodes = source.Elements().SkipWhile(i => i.Attribute("key").Value != searchElement.Attribute("key").Value);
            var commonNode = afterNodes.FirstOrDefault(i => XElementContainsChild(i, target));
            return target.Elements().FirstOrDefault(i => commonNode != null && i.Attribute("key").Value == commonNode.Attribute("key").Value);
        }

        private static XElement XElementGetChildWithKey(XElement parent, string key)
        {
            return parent.Elements().First(j => j.HasAttributes && j.Attribute("key") != null && j.Attribute("key").Value == key);
        }

        private static bool XElementIsLeafNode(XElement element)
        {
            return element.Name == "val";
        }

        private static bool XElementsAreKeyEqual(XElement source, XElement target)
        {
            if (source.Name != "item" || target.Name != "item")
            {
                return false;
            }
            var sourceAttr = XElementGetKeyAttribute(source);
            var targetAttr = XElementGetKeyAttribute(target);
            return sourceAttr != null && targetAttr != null && sourceAttr == targetAttr;
        }

        private static string XElementGetKeyAttribute(XElement element)
        {
            return element.HasAttributes && element.Attribute("key") != null ? element.Attribute("key").Value : null;
        }

        private const string itemLeafTemplate = @"<val m=""0"" l=""1""><![CDATA[{0}]]></val>";

        private static string GetAttributes(XElement xmlElement)
        {
            var str = "";
            if (xmlElement.HasAttributes)
            {
                str = xmlElement.Attributes().Aggregate(str, (current, i) => current + string.Format(@"{0}=""{1}"" ", i.Name, i.Value));
            }
            return string.IsNullOrEmpty(str.Trim()) ? "" : " " + str.Trim();
        }
    }
}
