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
            this._undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._printPreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._customizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._indexMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._searchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._newButton = new System.Windows.Forms.ToolStripButton();
            this._openButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this._printButton = new System.Windows.Forms.ToolStripButton();
            this._helpButton = new System.Windows.Forms.ToolStripButton();
            this._operationToolStrip = new System.Windows.Forms.ToolStrip();
            this._panToolBtn = new System.Windows.Forms.ToolStripButton();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._contextToolStrip = new System.Windows.Forms.ToolStrip();
            this._zoomText = new System.Windows.Forms.ToolStripTextBox();
            this._renderControl = new EscherTilier.RenderControl();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            sep3 = new System.Windows.Forms.ToolStripSeparator();
            editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sep4 = new System.Windows.Forms.ToolStripSeparator();
            sep5 = new System.Windows.Forms.ToolStripSeparator();
            this._menuStrip.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._operationToolStrip.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._contextToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new System.Drawing.Size(149, 6);
            // 
            // sep2
            // 
            sep2.Name = "sep2";
            sep2.Size = new System.Drawing.Size(149, 6);
            // 
            // sep3
            // 
            sep3.Name = "sep3";
            sep3.Size = new System.Drawing.Size(149, 6);
            // 
            // editMenuItem
            // 
            editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._undoMenuItem,
            this._redoMenuItem});
            editMenuItem.Name = "editMenuItem";
            editMenuItem.Size = new System.Drawing.Size(39, 20);
            editMenuItem.Text = "&Edit";
            editMenuItem.Visible = false;
            // 
            // _undoMenuItem
            // 
            this._undoMenuItem.Name = "_undoMenuItem";
            this._undoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this._undoMenuItem.Size = new System.Drawing.Size(144, 22);
            this._undoMenuItem.Text = "&Undo";
            this._undoMenuItem.Click += new System.EventHandler(this.undoMenuItem_Click);
            // 
            // _redoMenuItem
            // 
            this._redoMenuItem.Name = "_redoMenuItem";
            this._redoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this._redoMenuItem.Size = new System.Drawing.Size(144, 22);
            this._redoMenuItem.Text = "&Redo";
            this._redoMenuItem.Click += new System.EventHandler(this.redoMenuItem_Click);
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
            // _menuStrip
            // 
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            editMenuItem,
            this._toolsMenuItem,
            this._helpMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(784, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "Menu";
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newMenuItem,
            this._openMenuItem,
            sep1,
            this._saveMenuItem,
            this._saveAsMenuItem,
            sep2,
            this._printMenuItem,
            this._printPreviewMenuItem,
            sep3,
            this._exitMenuItem});
            this._fileMenuItem.Name = "_fileMenuItem";
            this._fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this._fileMenuItem.Text = "&File";
            // 
            // _newMenuItem
            // 
            this._newMenuItem.Image = global::EscherTilier.Properties.Resources.NewIcon;
            this._newMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newMenuItem.Name = "_newMenuItem";
            this._newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._newMenuItem.Size = new System.Drawing.Size(152, 22);
            this._newMenuItem.Text = "&New";
            this._newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // _openMenuItem
            // 
            this._openMenuItem.Image = global::EscherTilier.Properties.Resources.OpenIcon;
            this._openMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openMenuItem.Name = "_openMenuItem";
            this._openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openMenuItem.Size = new System.Drawing.Size(152, 22);
            this._openMenuItem.Text = "&Open";
            this._openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // _saveMenuItem
            // 
            this._saveMenuItem.Image = global::EscherTilier.Properties.Resources.SaveIcon;
            this._saveMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveMenuItem.Name = "_saveMenuItem";
            this._saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveMenuItem.Size = new System.Drawing.Size(152, 22);
            this._saveMenuItem.Text = "&Save";
            this._saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // _saveAsMenuItem
            // 
            this._saveAsMenuItem.Name = "_saveAsMenuItem";
            this._saveAsMenuItem.Size = new System.Drawing.Size(152, 22);
            this._saveAsMenuItem.Text = "Save &As";
            this._saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // _printMenuItem
            // 
            this._printMenuItem.Image = global::EscherTilier.Properties.Resources.PrintIcon;
            this._printMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printMenuItem.Name = "_printMenuItem";
            this._printMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this._printMenuItem.Size = new System.Drawing.Size(152, 22);
            this._printMenuItem.Text = "&Print";
            this._printMenuItem.Click += new System.EventHandler(this.printMenuItem_Click);
            // 
            // _printPreviewMenuItem
            // 
            this._printPreviewMenuItem.Image = global::EscherTilier.Properties.Resources.PrintPreviewIcon;
            this._printPreviewMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printPreviewMenuItem.Name = "_printPreviewMenuItem";
            this._printPreviewMenuItem.Size = new System.Drawing.Size(152, 22);
            this._printPreviewMenuItem.Text = "Print Pre&view";
            this._printPreviewMenuItem.Click += new System.EventHandler(this.printPreviewMenuItem_Click);
            // 
            // _exitMenuItem
            // 
            this._exitMenuItem.Name = "_exitMenuItem";
            this._exitMenuItem.Size = new System.Drawing.Size(152, 22);
            this._exitMenuItem.Text = "E&xit";
            this._exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // _toolsMenuItem
            // 
            this._toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._customizeMenuItem,
            this._optionsMenuItem});
            this._toolsMenuItem.Name = "_toolsMenuItem";
            this._toolsMenuItem.Size = new System.Drawing.Size(47, 20);
            this._toolsMenuItem.Text = "&Tools";
            this._toolsMenuItem.Visible = false;
            // 
            // _customizeMenuItem
            // 
            this._customizeMenuItem.Name = "_customizeMenuItem";
            this._customizeMenuItem.Size = new System.Drawing.Size(130, 22);
            this._customizeMenuItem.Text = "&Customize";
            // 
            // _optionsMenuItem
            // 
            this._optionsMenuItem.Name = "_optionsMenuItem";
            this._optionsMenuItem.Size = new System.Drawing.Size(130, 22);
            this._optionsMenuItem.Text = "&Options";
            // 
            // _helpMenuItem
            // 
            this._helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._indexMenuItem,
            this._searchMenuItem,
            sep4,
            this._aboutMenuItem});
            this._helpMenuItem.Name = "_helpMenuItem";
            this._helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this._helpMenuItem.Text = "&Help";
            // 
            // _indexMenuItem
            // 
            this._indexMenuItem.Name = "_indexMenuItem";
            this._indexMenuItem.Size = new System.Drawing.Size(116, 22);
            this._indexMenuItem.Text = "&Index";
            this._indexMenuItem.Click += new System.EventHandler(this.indexMenuItem_Click);
            // 
            // _searchMenuItem
            // 
            this._searchMenuItem.Name = "_searchMenuItem";
            this._searchMenuItem.Size = new System.Drawing.Size(116, 22);
            this._searchMenuItem.Text = "&Search";
            this._searchMenuItem.Click += new System.EventHandler(this.searchMenuItem_Click);
            // 
            // _aboutMenuItem
            // 
            this._aboutMenuItem.Name = "_aboutMenuItem";
            this._aboutMenuItem.Size = new System.Drawing.Size(116, 22);
            this._aboutMenuItem.Text = "&About...";
            this._aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newButton,
            this._openButton,
            this._saveButton,
            this._printButton,
            sep5,
            this._helpButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 24);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(784, 25);
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "Tools";
            // 
            // _newButton
            // 
            this._newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newButton.Image = global::EscherTilier.Properties.Resources.NewIcon;
            this._newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newButton.Name = "_newButton";
            this._newButton.Size = new System.Drawing.Size(23, 22);
            this._newButton.Text = "&New";
            this._newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // _openButton
            // 
            this._openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openButton.Image = global::EscherTilier.Properties.Resources.OpenIcon;
            this._openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(23, 22);
            this._openButton.Text = "&Open";
            this._openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // _saveButton
            // 
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveButton.Image = global::EscherTilier.Properties.Resources.SaveIcon;
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(23, 22);
            this._saveButton.Text = "&Save";
            this._saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // _printButton
            // 
            this._printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printButton.Image = global::EscherTilier.Properties.Resources.PrintIcon;
            this._printButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printButton.Name = "_printButton";
            this._printButton.Size = new System.Drawing.Size(23, 22);
            this._printButton.Text = "&Print";
            this._printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // _helpButton
            // 
            this._helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._helpButton.Image = global::EscherTilier.Properties.Resources.HelpIcon;
            this._helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._helpButton.Name = "_helpButton";
            this._helpButton.Size = new System.Drawing.Size(23, 22);
            this._helpButton.Text = "He&lp";
            this._helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // _operationToolStrip
            // 
            this._operationToolStrip.AutoSize = false;
            this._operationToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this._operationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._operationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._panToolBtn});
            this._operationToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this._operationToolStrip.Location = new System.Drawing.Point(0, 49);
            this._operationToolStrip.Name = "_operationToolStrip";
            this._operationToolStrip.Size = new System.Drawing.Size(41, 490);
            this._operationToolStrip.TabIndex = 2;
            this._operationToolStrip.Text = "Operations";
            // 
            // _panToolBtn
            // 
            this._panToolBtn.AutoSize = false;
            this._panToolBtn.Checked = true;
            this._panToolBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this._panToolBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._panToolBtn.Image = global::EscherTilier.Properties.Resources.PanTool_Icon;
            this._panToolBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._panToolBtn.Name = "_panToolBtn";
            this._panToolBtn.Size = new System.Drawing.Size(20, 20);
            this._panToolBtn.Text = "Pan";
            this._panToolBtn.Click += new System.EventHandler(this.toolBtn_Click);
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusInfoLabel});
            this._statusStrip.Location = new System.Drawing.Point(0, 539);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(784, 22);
            this._statusStrip.TabIndex = 3;
            this._statusStrip.Text = "Status";
            // 
            // _statusInfoLabel
            // 
            this._statusInfoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this._statusInfoLabel.Name = "_statusInfoLabel";
            this._statusInfoLabel.Size = new System.Drawing.Size(112, 17);
            this._statusInfoLabel.Text = "[Status Information]";
            // 
            // _contextToolStrip
            // 
            this._contextToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._contextToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._contextToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._zoomText});
            this._contextToolStrip.Location = new System.Drawing.Point(41, 514);
            this._contextToolStrip.Name = "_contextToolStrip";
            this._contextToolStrip.Size = new System.Drawing.Size(743, 25);
            this._contextToolStrip.TabIndex = 4;
            this._contextToolStrip.Text = "Context Tools";
            // 
            // _zoomText
            // 
            this._zoomText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._zoomText.AutoSize = false;
            this._zoomText.BackColor = System.Drawing.SystemColors.Window;
            this._zoomText.Name = "_zoomText";
            this._zoomText.Size = new System.Drawing.Size(50, 25);
            this._zoomText.Text = "100%";
            this._zoomText.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._zoomText.WordWrap = false;
            this._zoomText.Leave += new System.EventHandler(this.zoomText_Leave);
            // 
            // _renderControl
            // 
            this._renderControl.Cursor = System.Windows.Forms.Cursors.Cross;
            this._renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._renderControl.Location = new System.Drawing.Point(41, 49);
            this._renderControl.Name = "_renderControl";
            this._renderControl.Size = new System.Drawing.Size(743, 465);
            this._renderControl.TabIndex = 5;
            this._renderControl.Render += new EscherTilier.RenderDelegate(this.renderControl_Render);
            this._renderControl.Layout += new System.Windows.Forms.LayoutEventHandler(this.renderControl_Layout);
            this._renderControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseDown);
            this._renderControl.MouseLeave += new System.EventHandler(this.renderControl_MouseLeave);
            this._renderControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseMove);
            this._renderControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseUp);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._renderControl);
            this.Controls.Add(this._contextToolStrip);
            this.Controls.Add(this._operationToolStrip);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._menuStrip);
            this.Controls.Add(this._statusStrip);
            this.MainMenuStrip = this._menuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Escher Tililer";
            this.Click += new System.EventHandler(this.toolBtn_Click);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._operationToolStrip.ResumeLayout(false);
            this._operationToolStrip.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._contextToolStrip.ResumeLayout(false);
            this._contextToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

