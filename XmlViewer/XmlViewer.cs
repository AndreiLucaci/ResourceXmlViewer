using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XmlHandler.UnitTests;
using XmlHandler.XmlHandlerUtil;
using XmlViewerApp.XmlViewerEventArgs;

namespace XmlViewerApp
{
	public partial class XmlViewer : Form
	{
		//public event EventHandler TriggerSearch



		#region Fields

		private XElement _xmlElement;
		private XElement _originalRoot;

		#endregion

		public XmlViewer(string file)
		{
			InitializeComponent();

			loadFileToView.ProgressChanged += LoadFileToViewOnProgressChanged;
			loadFileToView.WorkerReportsProgress = true;
			loadFileToView.RunWorkerCompleted += LoadFileToViewOnRunWorkerCompleted;
			loadFileToView.DoWork += LoadFileToViewOnDoWork;

			FileName = file;
			if (string.IsNullOrEmpty(FileName))
			{
				_xmlElement = XmlUtil.CreateRootXElement();
				return;
			}
			try
			{
				LoadFile();
			}
			catch (Exception ex)
			{
				FormHelper.Error(ex.Message);
				_xmlElement = XmlUtil.CreateRootXElement();
			}

			Text = FileName;

			var lbl = new ToolStripLabel("Search: ");
			var tb = new ToolStripTextBox()
			{
				//RightToLeft = lbl.RightToLeft = RightToLeft.Yes,
				//Width = 5000,
				Dock = DockStyle.Fill,
				AutoSize = false
			};
			tb.KeyUp += (o, e) =>
			{
				if (e.KeyCode == Keys.Enter)
				{
					var text = tb.Text;
					var parts = text.Split('.');
					if (parts.Count() == 1)
					{
						_searchElements.Clear();
						_searchElementsCounter = -1;
						_searchElements.AddRange(_originalRoot.Descendants().Where(i => i.Name == "val" && i.Value.Contains(text)));

						CycleSearch();
					}
					else
					{
						var rootNode = treeView1.Nodes[0];
						SelectNodeFromTree(text, rootNode);
					}
				}
				if (e.Control && e.KeyCode == Keys.O)
				{
					(Parent as MainWindow).OpenFile();
				}
			};
			statusStrip1.Items.Add(lbl);
			statusStrip1.Items.Add(tb);

			xmlViewerOptions1.RefreshFile += (e, o) =>
			{
				ClearFileFromMemory();
				LoadFile();
			};

		}

		private void LoadFile()
		{
			new Thread(() =>
			{
				try
				{
					if (FileName != null)
					{
						_xmlElement = XElement.Parse(XmlUtil.GetStringFromFile(FileName), LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
						new Thread(() =>
						{
							_originalRoot = new XElement(_xmlElement);
						}).Start();

						xmlViewerOptions1.Root = _xmlElement;
						SetRichTextBoxText(); 
					} else
					{
						throw new Exception("There is no file to be loaded");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}).Start();
		}

		public void CycleSearch()
		{
			var leafItem = GetNextSearchXElement();
			if (leafItem != null)
			{
				SlectNodeFromSearch(leafItem);
			}
		}

		private List<XElement> _searchElements = new List<XElement>();
		private int _searchElementsCounter = -1;

		private XElement GetNextSearchXElement()
		{
			if (_searchElements.Count == 0) return null;
			if (_searchElementsCounter + 1 == _searchElements.Count || _searchElementsCounter == -1)
			{
				_searchElementsCounter = 0;
			}
			return _searchElements[_searchElementsCounter++];
		}

		private void SlectNodeFromSearch(XElement leafItem)
		{
			var currentPath = new List<string>();
			var root = leafItem.Parent;
			while (root != null && root.Name != "resources")
			{
				currentPath.Add(root.Attribute("key").Value);
				root = root.Parent;
			}
			currentPath.Reverse();
			var path = string.Join(".", currentPath);
			var rootNode = treeView1.Nodes[0];
			SelectNodeFromTree(path, rootNode);
		}

		private void LoadFileToViewOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			FormHelper.InvokeControl(xmlViewerOptions1, () => xmlViewerOptions1.Enabled = false);
			InitializeTreeViewAsync();
		}

		private void LoadFileToViewOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
		{
			var progBar = statusStrip1.Items.Find("ProgBar", false).FirstOrDefault();
			if (progBar != null)
				FormHelper.InvokeControl(statusStrip1, () => statusStrip1.Items.Remove(progBar));

			FormHelper.InvokeControl(xmlViewerOptions1, () => xmlViewerOptions1.Enabled = true);
		}

		private void LoadFileToViewOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			var progBar = statusStrip1.Items.Find("ProgBar", false).FirstOrDefault() as ToolStripProgressBar;
			if (progBar != null)
			{
				progBar.Value = progressChangedEventArgs.ProgressPercentage;
			}
		}

		private void SetRichTextBoxText()
		{
			LoadFileInBackground();
		}

		public string FileName { get; set; }

		private void XmlViewer_DragDrop(object sender, DragEventArgs e)
		{

		}


		private void LoadFile(bool reportProgress = true)
		{
			var sb = XmlUtil.FormatXElement(_xmlElement);
			var sbLen = sb.Length;
			for (var i = 0; i < sbLen; i += bufferSize)
			{
				var buff = sbLen - (i + bufferSize) < 0 ? sbLen - i : bufferSize;
				var chunk = new char[buff];
				sb.CopyTo(i, chunk, 0, buff);
				if (reportProgress)
				{
					loadFileToView.ReportProgress((i * 100) / sbLen);
				}
			}
		}

		private void LoadFileInBackground()
		{
			loadFileToView.RunWorkerAsync();
		}

		private const int bufferSize = 16384;

		private void xmlViewerOptions1_Load(object sender, EventArgs e)
		{

		}

		private void InitializeTreeView()
		{
			try
			{
				treeView1.Nodes.Clear();
				var root = new TreeNode(_xmlElement.Name.LocalName);
				treeView1.Nodes.Add(root);
				PopulateTreeNode(root, _xmlElement);
				if (treeView1.Nodes.Count > 0)
					treeView1.Nodes[0].Expand();
			}
			catch (Exception ex)
			{
				FormHelper.Error(ex.Message);
			}
		}

		private void InitializeTreeViewAsync()
		{
			try
			{
				FormHelper.InvokeControl(treeView1, () =>
				{
					treeView1.Nodes.Clear();
					var root = new TreeNode(_xmlElement.Name.LocalName);
					treeView1.Nodes.Add(root);
					PopulateTreeNode(root, _xmlElement);
					if (treeView1.Nodes.Count > 0)
						treeView1.Nodes[0].Expand();
				});
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
				foreach (XElement i in currentXElement.Elements())
				{
					if (i.Name == "val") return;
					var treeNode = new TreeNode(XmlUtil.FormatXElementNoTags(i));
					treeNode.Name = XmlUtil.GenerateXElementPath(i);
					currentTreeNode.Nodes.Add(treeNode);
					PopulateTreeNode(treeNode, i);
				}
			}
		}

		private void xmlViewerOptions1_SearchKey(object sender, XmlViewerOptionsSearchKeyEventArgs e)
		{
			var key = e.Key;

			if (treeView1.Nodes.Count < 1)
			{
				return;
			}
			var rootNode = treeView1.Nodes[0];
			SelectNodeFromTree(key, rootNode);

		}

		private void SelectNodeFromTree(string path, TreeNode currentTreeNode)
		{
			ResetColorNode();
			var parts = path.Split('.');
			var pathUntilNow = "";
			foreach (var i in parts)
			{
				pathUntilNow += i + ".";
				currentTreeNode = currentTreeNode.Nodes.Find(pathUntilNow.Trim('.'), false).FirstOrDefault();
				if (currentTreeNode == null) return;
			}
			CollapseSiblings(currentTreeNode);
			CollapseChildren(currentTreeNode);
			currentTreeNode.Expand();
			currentTreeNode.EnsureVisible();
			ColorNode(currentTreeNode);
			treeView1.SelectedNode = currentTreeNode;
		}

		private void CollapseSiblings(TreeNode treeNode)
		{
			var parent = treeNode.Parent;
			if (parent == null) return;
			foreach (var i in from TreeNode i in parent.Nodes where i.Name != treeNode.Name select i)
			{
				i.Collapse();
			}
		}

		private void CollapseChildren(TreeNode treeNode)
		{
			foreach (TreeNode i in treeNode.Nodes)
			{
				i.Collapse();
			}
		}

		private void ColorNode(TreeNode treeNode)
		{
			previouslyExpandedNode = treeNode;
			var node = previouslyExpandedNode;
			while (node.Parent != null)
			{
				node.BackColor = Color.Black;
				node.ForeColor = Color.White;
				node = node.Parent;
			}
		}

		private TreeNode previouslyExpandedNode = null;

		private void ResetColorNode()
		{
			if (previouslyExpandedNode != null)
			{
				var node = previouslyExpandedNode;
				while (node.Parent != null)
				{
					node.BackColor = Color.White;
					node.ForeColor = Color.Black;
					node = node.Parent;
				}
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var path = CreatePathFromTreeView(e.Node);
			e.Node.Expand();
			ResetColorNode();
			CollapseSiblings(e.Node);
			CollapseChildren(e.Node);
			ColorNode(e.Node);
			xmlViewerOptions1.SetSearchBoxPartialKey(path, XmlUtil.GetXElement(path, _xmlElement));
			currentKeyStatusLabel.Text = path;
		}

		private string CreatePathFromTreeView(TreeNode currentTreeNode)
		{
			var treeNode = currentTreeNode;
			var path = "";
			while (treeNode.Parent != null)
			{
				path = string.Format("{0}.{1}", path,
					Regex.Match(treeNode.Text, "item key=\"(.*)\"").Groups[1].ToString());
				treeNode = treeNode.Parent;
			}
			return string.Join(".", path.Trim('.').Split('.').Reverse());
		}

		private void treeView1_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void treeView1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var treeInfoNode = treeView1.HitTest(e.X, e.Y);
				EventHandler del = null;
				del = delegate (object snd, EventArgs ev)
				{
                    var toolStrip = (snd as ToolStripMenuItem);
					var pathToNode = treeInfoNode.Node.Name;
					var lineNumber = XmlUtil.GetLineNumberOfXElement(pathToNode, _xmlElement);
					Util.StartEditApplication(FileName, lineNumber, toolStrip.Equals(openInSublimeToolStripMenuItem) ? Software.SublimeText3 : Software.Notepad);
					toolStrip.Click -= del;
				};
				toolStripMenuItem1.Click += del;
                openInSublimeToolStripMenuItem.Click += del;
				contextMenuStrip1.Show(treeView1, new Point(e.X, e.Y));
			}
		}

		private void XmlViewer_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				CycleSearch();
			}
			if (e.Control && e.KeyCode == Keys.O)
			{
				(Parent as MainWindow).OpenFile();
			}
		}

		private void XmlViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			ClearFileFromMemory();
			_searchElements.ForEach(i => i.RemoveAll());
		}

		private void ClearFileFromMemory()
		{
			_originalRoot?.RemoveAll();
			_xmlElement?.RemoveAll();
		}

        private void openInSublimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
