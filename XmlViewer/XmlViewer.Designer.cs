namespace XmlViewerApp
{
    partial class XmlViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlViewer));
			this.panel1 = new System.Windows.Forms.Panel();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.xmlViewerOptions1 = new XmlViewerApp.XmlViewerOptions();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.currentKeyStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.loadFileToView = new System.ComponentModel.BackgroundWorker();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.openInSublimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.treeView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(187, 430);
			this.panel1.TabIndex = 0;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(187, 430);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
			this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.xmlViewerOptions1);
			this.splitContainer1.Size = new System.Drawing.Size(910, 430);
			this.splitContainer1.SplitterDistance = 187;
			this.splitContainer1.TabIndex = 1;
			// 
			// xmlViewerOptions1
			// 
			this.xmlViewerOptions1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xmlViewerOptions1.Location = new System.Drawing.Point(0, 0);
			this.xmlViewerOptions1.Name = "xmlViewerOptions1";
			this.xmlViewerOptions1.Root = null;
			this.xmlViewerOptions1.Size = new System.Drawing.Size(719, 430);
			this.xmlViewerOptions1.TabIndex = 0;
			this.xmlViewerOptions1.SearchKey += new System.EventHandler<XmlViewerApp.XmlViewerEventArgs.XmlViewerOptionsSearchKeyEventArgs>(this.xmlViewerOptions1_SearchKey);
			this.xmlViewerOptions1.Load += new System.EventHandler(this.xmlViewerOptions1_Load);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentKeyStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 430);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(910, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// currentKeyStatusLabel
			// 
			this.currentKeyStatusLabel.Name = "currentKeyStatusLabel";
			this.currentKeyStatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.openInSublimeToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(182, 48);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Image = global::XmlViewer.Properties.Resources._2064__Notepad5_icon;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
			this.toolStripMenuItem1.Text = "Open in Notepad++";
			// 
			// openInSublimeToolStripMenuItem
			// 
			this.openInSublimeToolStripMenuItem.Name = "openInSublimeToolStripMenuItem";
			this.openInSublimeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.openInSublimeToolStripMenuItem.Text = "Open in Sublime";
			this.openInSublimeToolStripMenuItem.Click += new System.EventHandler(this.openInSublimeToolStripMenuItem_Click);
			// 
			// XmlViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(910, 452);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "XmlViewer";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XmlViewer_FormClosing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.XmlViewer_DragDrop);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.XmlViewer_KeyUp);
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.ComponentModel.BackgroundWorker loadFileToView;
        private XmlViewerApp.XmlViewerOptions xmlViewerOptions1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripStatusLabel currentKeyStatusLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openInSublimeToolStripMenuItem;
    }
}

