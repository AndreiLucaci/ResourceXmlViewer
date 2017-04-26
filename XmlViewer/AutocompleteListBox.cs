using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using XmlHandler.XmlHandlerUtil;

namespace XmlViewerApp
{
    public partial class AutocompleteListBox : UserControl
    {
        private XElement _xmlElement;

        public AutocompleteListBox()
        {
            InitializeComponent();
        }

        public void InitializeAutoComplete(TextBox searchBox, XElement xmlElement)
        {
            InitializeComponent();
            searchBox.TextChanged += SearchBoxOnTextChanged;
            _xmlElement = xmlElement;
        }

        private void SearchBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            if (!Visible) return;
            var textBox = sender as TextBox;
            if (textBox == null || string.IsNullOrEmpty(textBox.Text) || string.IsNullOrWhiteSpace(textBox.Text)) return;
            var items = XmlUtil.GetAllXElementsKeysStartingWith(textBox.Text, _xmlElement);
            if (items == null || items.Count == 0)
            {
                FormHelper.InvokeControl(autocompleteBox, NoItemFound);
            }
            else
            {
                FormHelper.InvokeControl(autocompleteBox, () => PopulateAutoCompleteBox(items));
            }
            FormHelper.InvokeControl(this, Invalidate);
        }


        public void PopulateAutoCompleteBox(List<string> items)
        {
            autocompleteBox.DataSource = null;
            autocompleteBox.DataSource = items;
        }

        public void NoItemFound()
        {
            var lst = new List<string> {"No Items found"};
            autocompleteBox.DataSource = null;
            autocompleteBox.DataSource = lst;
        }

        private void autocompleteBox_DoubleClick(object sender, EventArgs e)
        {
            AutocompleteChangeKey();
        }

        private void AutocompleteChangeKey()
        {
            var text = GetSelectedKey();
            if (SearchKeySelected != null)
            {
                SearchKeySelected(this, new AutoCompleteSearchKeyEventArgs {Key = text});
            }
        }

        public event EventHandler<AutoCompleteSearchKeyEventArgs> SearchKeySelected;

        public void MoveSelectionDown()
        {
            if (autocompleteBox.SelectedIndex < 0 || autocompleteBox.SelectedIndex + 1 == autocompleteBox.Items.Count)
                return;
            autocompleteBox.SelectedIndex++;
        }

        public void MoveSelectionUp()
        {
            if (autocompleteBox.SelectedIndex <= 0)
                return;
            autocompleteBox.SelectedIndex--;
        }

        public string GetSelectedKey()
        {
            if (autocompleteBox.SelectedIndex < 0) return "";
            var text = autocompleteBox.SelectedItem;
            return text.ToString();
        }

        private void AutocompleteListBox_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                autocompleteBox.DataSource = null;
            }
        }
    }
    
    public class AutoCompleteSearchKeyEventArgs : EventArgs
    {
        public string Key { get; set; }
    }
}
