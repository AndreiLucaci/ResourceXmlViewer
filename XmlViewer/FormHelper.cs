using System;
using System.Windows.Forms;

namespace XmlViewerApp
{
    public static class FormHelper
    {
        public static void Error(string errorMessage)
        {
            MessageBox.Show(errorMessage, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void InvokeControl(Control control, Action action)
        {
            try
            {
                control.Invoke(action);
            }
            catch
            {
            }
        }
    }
}