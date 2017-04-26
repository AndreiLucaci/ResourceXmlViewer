using System;

namespace XmlViewerApp.XmlViewerEventArgs
{
    public class XmlViewerOptionsWordWrapEventArgs : EventArgs
    {
        public bool Checked { get; set; }
    }

    public class XmlViewerOptionsSearchKeyEventArgs : EventArgs
    {
        public string Key { get; set; }
    }
}