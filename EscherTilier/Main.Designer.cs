namespace EscherTilier
{
    partial class Main
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
            System.Windows.Forms.ToolStripSeparator sep1;
            System.Windows.Forms.ToolStripSeparator sep2;
            System.Windows.Forms.ToolStripSeparator sep3;
            System.Windows.Forms.ToolStripMenuItem editMenuItem;
            System.Windows.Forms.ToolStripSeparator sep4;
            System.Windows.Forms.ToolStripSeparator sep5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newButton = new System.Windows.Forms.ToolStripButton();
            this.openButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.printButton = new System.Windows.Forms.ToolStripButton();
            this.helpButton = new System.Windows.Forms.ToolStripButton();
            this.operationToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextToolStrip = new System.Windows.Forms.ToolStrip();
            this.renderControl = new EscherTilier.RenderControl();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            sep3 = new System.Windows.Forms.ToolStripSeparator();
            editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sep4 = new System.Windows.Forms.ToolStripSeparator();
            sep5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.operationToolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new System.Drawing.Size(143, 6);
            // 
            // sep2
            // 
            sep2.Name = "sep2";
            sep2.Size = new System.Drawing.Size(143, 6);
            // 
            // sep3
            // 
            sep3.Name = "sep3";
            sep3.Size = new System.Drawing.Size(143, 6);
            // 
            // editMenuItem
            // 
            editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem});
            editMenuItem.Name = "editMenuItem";
            editMenuItem.Size = new System.Drawing.Size(39, 20);
            editMenuItem.Text = "&Edit";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoMenuItem.Text = "&Undo";
            this.undoMenuItem.Click += new System.EventHandler(this.undoMenuItem_Click);
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoMenuItem.Text = "&Redo";
            this.redoMenuItem.Click += new System.EventHandler(this.redoMenuItem_Click);
            // 
            // sep4
            // 
            sep4.Name = "sep4";
            sep4.Size = new System.Drawing.Size(113, 6);
            // 
            // sep5
            // 
            sep5.Name = "sep5";
            sep5.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            editMenuItem,
            this.toolsMenuItem,
            this.helpMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "Menu";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            sep1,
            this.saveMenuItem,
            this.saveAsMenuItem,
            sep2,
            this.printMenuItem,
            this.printPreviewMenuItem,
            sep3,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "&File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newMenuItem.Image")));
            this.newMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newMenuItem.Text = "&New";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openMenuItem.Image")));
            this.openMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openMenuItem.Text = "&Open";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveMenuItem.Image")));
            this.saveMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsMenuItem.Text = "Save &As";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // printMenuItem
            // 
            this.printMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printMenuItem.Image")));
            this.printMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printMenuItem.Text = "&Print";
            this.printMenuItem.Click += new System.EventHandler(this.printMenuItem_Click);
            // 
            // printPreviewMenuItem
            // 
            this.printPreviewMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewMenuItem.Image")));
            this.printPreviewMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewMenuItem.Name = "printPreviewMenuItem";
            this.printPreviewMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printPreviewMenuItem.Text = "Print Pre&view";
            this.printPreviewMenuItem.Click += new System.EventHandler(this.printPreviewMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeMenuItem,
            this.optionsMenuItem});
            this.toolsMenuItem.Name = "toolsMenuItem";
            this.toolsMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsMenuItem.Text = "&Tools";
            // 
            // customizeMenuItem
            // 
            this.customizeMenuItem.Name = "customizeMenuItem";
            this.customizeMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeMenuItem.Text = "&Customize";
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Name = "optionsMenuItem";
            this.optionsMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsMenuItem.Text = "&Options";
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexMenuItem,
            this.searchMenuItem,
            sep4,
            this.aboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpMenuItem.Text = "&Help";
            // 
            // indexMenuItem
            // 
            this.indexMenuItem.Name = "indexMenuItem";
            this.indexMenuItem.Size = new System.Drawing.Size(116, 22);
            this.indexMenuItem.Text = "&Index";
            this.indexMenuItem.Click += new System.EventHandler(this.indexMenuItem_Click);
            // 
            // searchMenuItem
            // 
            this.searchMenuItem.Name = "searchMenuItem";
            this.searchMenuItem.Size = new System.Drawing.Size(116, 22);
            this.searchMenuItem.Text = "&Search";
            this.searchMenuItem.Click += new System.EventHandler(this.searchMenuItem_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutMenuItem.Text = "&About...";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.openButton,
            this.saveButton,
            this.printButton,
            sep5,
            this.helpButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "Tools";
            // 
            // newButton
            // 
            this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newButton.Image = ((System.Drawing.Image)(resources.GetObject("newButton.Image")));
            this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(23, 22);
            this.newButton.Text = "&New";
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // openButton
            // 
            this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
            this.openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(23, 22);
            this.openButton.Text = "&Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "&Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // printButton
            // 
            this.printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printButton.Image = ((System.Drawing.Image)(resources.GetObject("printButton.Image")));
            this.printButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(23, 22);
            this.printButton.Text = "&Print";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpButton.Image = ((System.Drawing.Image)(resources.GetObject("helpButton.Image")));
            this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(23, 22);
            this.helpButton.Text = "He&lp";
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // operationToolStrip
            // 
            this.operationToolStrip.AutoSize = false;
            this.operationToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.operationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.operationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton1});
            this.operationToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.operationToolStrip.Location = new System.Drawing.Point(0, 49);
            this.operationToolStrip.Name = "operationToolStrip";
            this.operationToolStrip.Size = new System.Drawing.Size(41, 490);
            this.operationToolStrip.TabIndex = 2;
            this.operationToolStrip.Text = "Operations";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoSize = false;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton2.Text = "toolStripButton1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(20, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusInfoLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "Status";
            // 
            // statusInfoLabel
            // 
            this.statusInfoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.statusInfoLabel.Name = "statusInfoLabel";
            this.statusInfoLabel.Size = new System.Drawing.Size(112, 17);
            this.statusInfoLabel.Text = "[Status Information]";
            // 
            // contextToolStrip
            // 
            this.contextToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.contextToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.contextToolStrip.Location = new System.Drawing.Point(41, 514);
            this.contextToolStrip.Name = "contextToolStrip";
            this.contextToolStrip.Size = new System.Drawing.Size(743, 25);
            this.contextToolStrip.TabIndex = 4;
            this.contextToolStrip.Text = "Context Tools";
            // 
            // renderControl
            // 
            this.renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl.Location = new System.Drawing.Point(41, 49);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(743, 465);
            this.renderControl.TabIndex = 5;
            this.renderControl.RenderTargetChanged += new System.Action<SharpDX.Direct2D1.RenderTarget>(this.renderControl_RenderTargetChanged);
            this.renderControl.Render += new EscherTilier.RenderDelegate(this.renderControl_Render);
            this.renderControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseMove);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.renderControl);
            this.Controls.Add(this.contextToolStrip);
            this.Controls.Add(this.operationToolStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Escher Tililer";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.operationToolStrip.ResumeLayout(false);
            this.operationToolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripButton printButton;
        private System.Windows.Forms.ToolStripButton helpButton;
        private System.Windows.Forms.ToolStrip operationToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusInfoLabel;
        private System.Windows.Forms.ToolStrip contextToolStrip;
        private EscherTilier.RenderControl renderControl;
    }
}

