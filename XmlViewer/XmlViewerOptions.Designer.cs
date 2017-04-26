namespace XmlViewerApp
{
    partial class XmlViewerOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.searchBox = new System.Windows.Forms.TextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.autocompleteListBox1 = new XmlViewerApp.AutocompleteListBox();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.button1);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(251, 38);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(3, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 35);
			this.button1.TabIndex = 0;
			this.button1.Text = "Copy key to clipboard";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// searchBox
			// 
			this.searchBox.AcceptsTab = true;
			this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.searchBox.Location = new System.Drawing.Point(3, 44);
			this.searchBox.Name = "searchBox";
			this.searchBox.Size = new System.Drawing.Size(245, 20);
			this.searchBox.TabIndex = 1;
			this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
			this.searchBox.Enter += new System.EventHandler(this.searchBox_Enter);
			this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
			this.searchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyUp);
			this.searchBox.Leave += new System.EventHandler(this.searchBox_Leave);
			this.searchBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.searchBox_PreviewKeyDown);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(3, 70);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(242, 274);
			this.richTextBox1.TabIndex = 4;
			this.richTextBox1.Text = "";
			this.richTextBox1.WordWrap = false;
			// 
			// autocompleteListBox1
			// 
			this.autocompleteListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.autocompleteListBox1.BackColor = System.Drawing.SystemColors.Window;
			this.autocompleteListBox1.Location = new System.Drawing.Point(3, 65);
			this.autocompleteListBox1.Name = "autocompleteListBox1";
			this.autocompleteListBox1.Size = new System.Drawing.Size(242, 72);
			this.autocompleteListBox1.TabIndex = 3;
			this.autocompleteListBox1.Visible = false;
			this.autocompleteListBox1.SearchKeySelected += new System.EventHandler<XmlViewerApp.AutoCompleteSearchKeyEventArgs>(this.autocompleteListBox1_SearchKeySelected);
			this.autocompleteListBox1.Leave += new System.EventHandler(this.autocompleteListBox1_Leave);
			// 
			// XmlViewerOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.autocompleteListBox1);
			this.Controls.Add(this.searchBox);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.richTextBox1);
			this.Name = "XmlViewerOptions";
			this.Size = new System.Drawing.Size(251, 347);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button button1;
        private global::XmlViewerApp.AutocompleteListBox autocompleteListBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
