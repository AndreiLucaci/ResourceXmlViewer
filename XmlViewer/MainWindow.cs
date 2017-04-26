using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace XmlViewerApp
{
    public partial class MainWindow : Form
    {
        private List<XmlViewer> _xmlViewers = new List<XmlViewer>();
        private XmlViewer _actiViewer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void createNewViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateXmlViewerForm("untitled.xml");
        }

        private void CreateXmlViewerForm(string file)
        {
            var xmlViewerForm = new XmlViewer(file) {MdiParent = this};
            xmlViewerForm.GotFocus += (sender, args) =>
            {
                _actiViewer = (sender as XmlViewer);
            };
            xmlViewerForm.Show();
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (!file.EndsWith(".xml"))
                {
                    FormHelper.Error("Invalid file provided. Need .xml file");
                    continue;
                }
                CreateXmlViewerForm(file);
            }
        }

        private void openResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        private void OpenFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (var i in openFileDialog1.FileNames)
                    {
                        CreateXmlViewerForm(i);
                    }
                }
                catch (Exception ex)
                {
                    FormHelper.Error(ex.Message);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new XmlViewerAboutBox();
            about.Show();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (_actiViewer != null)
                {
                    _actiViewer.CycleSearch();
                }
            }
        }
    }
}
