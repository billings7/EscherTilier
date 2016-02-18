using EscherTilier.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscherTilier
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            statusInfoLabel.Text = string.Empty;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            switch (Settings.Default.WindowState)
            {
                case FormWindowState.Maximized:
                    WindowState = FormWindowState.Maximized;
                    break;
                default:
                    WindowState = FormWindowState.Normal;
                    break;
            }
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            Settings.Default.WindowState = WindowState;

            if (WindowState != FormWindowState.Maximized)
            {
                Settings.Default.WindowSize = Size;
            }

            Settings.Default.Save();
        }
        
        #region File menu
        private void newMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printPreviewMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // TODO Prompt for save if unsaved

            Close();
        }
        #endregion

        #region Edit menu
        private void undoMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void redoMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Help menu
        private void indexMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void searchMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tool Strip
        private void newButton_Click(object sender, EventArgs e)
        {

        }

        private void openButton_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void printButton_Click(object sender, EventArgs e)
        {

        }

        private void helpButton_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
