using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XmlHandler.XmlHandlerUtil;
using XmlViewerApp.XmlViewerEventArgs;

namespace XmlViewerApp
{
    public partial class XmlViewerOptions : UserControl
    {
        public event EventHandler<XmlViewerOptionsSearchKeyEventArgs> SearchKey;

        private XElement _root;

        public XElement Root
        {
            get { return _root; }
            set
            {
                _root = value;
            }
        }


        private bool _showAutoCompleteBox;

        public XmlViewerOptions()
        {
            InitializeComponent();
        }
        
        public XmlViewerOptions(XElement xElement)
        {
            InitializeComponent();
            Root = xElement;
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Space)
            {
                StopPropagation(e);
                ShowAutoCompleteBox();
                PopulateAutocompleteBox();
            }
            if (e.KeyCode == Keys.Space)
            {
                StopPropagation(e);
            }
            if (e.KeyCode == Keys.Tab && _showAutoCompleteBox)
            {
                StopPropagation(e);
                SetSearchKeyFromAutocompleteBox();
            }
            if (e.KeyCode == Keys.Escape)
            {
                StopPropagation(e);
                HideAutoCompleteBox();
            }
            if (e.KeyCode == Keys.Down && _showAutoCompleteBox)
            {
                StopPropagation(e);
                autocompleteListBox1.MoveSelectionDown();

            }
            if (e.KeyCode == Keys.Up && _showAutoCompleteBox)
            {
                StopPropagation(e);
                autocompleteListBox1.MoveSelectionUp();
            }
        }

        private void StopPropagation(KeyEventArgs e)
        {
            e.Handled = e.SuppressKeyPress = true;            
        }

        private void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SearchKey != null)
                {
                    if (_showAutoCompleteBox)
                    {
                        SetSearchKeyFromAutocompleteBox();
                    }
                    searchBox.SelectionStart = searchBox.Text.Length;
                    searchBox.ScrollToCaret();
                    SearchKey(this, new XmlViewerOptionsSearchKeyEventArgs {Key = searchBox.Text});
                    HideAutoCompleteBox();
                }
                
            }
            
        }

        private void SetSearchKeyFromAutocompleteBox()
        {
            var text = autocompleteListBox1.GetSelectedKey();
            if (text != "No Items found")
            {
                searchBox.Text = text;
                searchBox.SelectionStart = text.Length;
            }
        }

        private void ShowAutoCompleteBox()
        {
            _showAutoCompleteBox = autocompleteListBox1.Visible = true;
        }

        public void SetSearchBoxPartialKey(string text, XElement xElement)
        {
            searchBox.Text = text;
            _xmlElement = xElement;
            if (_xmlElement != null && _xmlElement.HasElements && _xmlElement.Elements().First().Name == "val")
            {
                //InitializeTreeViewAsync();
                InitializeRichTextBoxAsync();
            }
            else
            {
                //treeView1.Nodes.Clear();
            }
        }

        private void InitializeRichTextBoxAsync()
        {
            FormHelper.InvokeControl(richTextBox1, () =>
            {
                var sb = new StringBuilder();
                var xw = XmlWriter.Create(sb,new XmlWriterSettings{Indent = true, IndentChars = "\t"} );
                _xmlElement.Save(xw);
                xw.Close();
                richTextBox1.Text = sb.ToString();
            });
        }

        private XElement _xmlElement;

        private void InitializeTreeViewAsync()
        {
            try
            {
                //FormHelper.InvokeControl(treeView1, () =>
                //{
                //    treeView1.Nodes.Clear();
                //    var root = new TreeNode(XmlUtil.FormatXElementNoTags(_xmlElement));
                //    treeView1.Nodes.Add(root);
                //    PopulateTreeNode(root, _xmlElement);
                //    if (treeView1.Nodes.Count > 0)
                //        treeView1.Nodes[0].Expand();
                //});
            }
            catch (Exception ex)
            {
                FormHelper.Error(ex.Message);
            }

        }


        private void PopulateTreeNode(TreeNode currentTreeNode, XElement currentXElement)
        {
            if (currentXElement.HasElements)
            {
                foreach (var i in currentXElement.Elements())
                {
                    var treeNode = new TreeNode(XmlUtil.GenerateFormatedXElement(i));
                    treeNode.Expand();
                    treeNode.ToolTipText = "";
                    currentTreeNode.Nodes.Add(treeNode);
                    PopulateTreeNode(treeNode, i);
                }
            }
            else
            {
                var innerValue = currentXElement.Nodes().Any() ? currentXElement.Nodes().First().ToString() : "";
                var treeNode =
                    new TreeNode(innerValue);
                currentTreeNode.Nodes.Add(treeNode);
                currentTreeNode.Text = XmlUtil.FormatXElementNoTags(currentXElement);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = searchBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return;
            searchBox.Focus();
            searchBox.SelectionStart = 0;
            searchBox.SelectionLength = text.Length;
            Clipboard.SetText(text);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            //HideAutoCompleteBox();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            PopulateAutocompleteBox();
        }

        private void PopulateAutocompleteBox()
        {
            if (_showAutoCompleteBox && (!string.IsNullOrEmpty(searchBox.Text) && !string.IsNullOrWhiteSpace(searchBox.Text)))
            {
                autocompleteListBox1.Visible = true;
                var items = XmlUtil.GetAllXElementsKeysStartingWith(searchBox.Text, _root);
                if (items == null || items.Count == 0)
                {
                    NoItemFound();
                }
                else
                {
                    PopulatelistBox1(items);
                }
            }
        }

        private void searchBox_Enter(object sender, EventArgs e)
        {
        }

        private void PopulatelistBox1(List<string> items)
        {
            if (_showAutoCompleteBox)
            {
                autocompleteListBox1.PopulateAutoCompleteBox(items);
            }
        }

        private void NoItemFound()
        {
            if (_showAutoCompleteBox)
            {
                autocompleteListBox1.NoItemFound();
            }
        }

        private void autocompleteListBox1_SearchKeySelected(object sender, AutoCompleteSearchKeyEventArgs e)
        {
            searchBox.Text = e.Key;
            HideAutoCompleteBox();
        }

        private void HideAutoCompleteBox()
        {
            _showAutoCompleteBox = autocompleteListBox1.Visible = false;
        }

        private void autocompleteListBox1_Leave(object sender, EventArgs e)
        {
            HideAutoCompleteBox();
        }

        private void searchBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.C)
            //{
            //    var selectedNode = treeView1.SelectedNode;
            //    if (selectedNode == null) return;
            //    Clipboard.SetText(selectedNode.Text);
            //}
        }
    }
}
