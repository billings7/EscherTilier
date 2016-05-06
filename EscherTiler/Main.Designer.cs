using System.Windows.Forms;
using EscherTiler.Graphics.GDI;
using JetBrains.Annotations;

namespace EscherTiler
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator sep1;
            System.Windows.Forms.ToolStripSeparator sep2;
            System.Windows.Forms.ToolStripSeparator sep3;
            System.Windows.Forms.ToolStripMenuItem _editMenuItem;
            System.Windows.Forms.ToolStripSeparator sep4;
            System.Windows.Forms.ToolStripSeparator sep5;
            System.Windows.Forms.Panel _stylesPanel;
            System.Windows.Forms.Label _seedLbl;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this._undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._fillStylesGroup = new System.Windows.Forms.GroupBox();
            this._styleList = new EscherTiler.StyleListView();
            this._lineStyleGroup = new System.Windows.Forms.GroupBox();
            this._lineStyleControl = new EscherTiler.StyleControl();
            this._lineWidthTrack = new System.Windows.Forms.TrackBar();
            this._greedyStyleManagerPnl = new System.Windows.Forms.Panel();
            this._greedyParamCTrack = new System.Windows.Forms.TrackBar();
            this._greedyParamBTrack = new System.Windows.Forms.TrackBar();
            this._greedyParamATrack = new System.Windows.Forms.TrackBar();
            this._randomStyleMangerPnl = new System.Windows.Forms.Panel();
            this._randomSeedBtn = new System.Windows.Forms.Button();
            this._seedNum = new System.Windows.Forms.NumericUpDown();
            this._styleManagerTypeCmb = new System.Windows.Forms.ComboBox();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pageSetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this._changeLineTypeCmb = new System.Windows.Forms.ToolStripComboBox();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._renderControl = new EscherTiler.RenderControl();
            this._pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this._printDocument = new EscherTiler.Graphics.GDI.TilerPrintDocument();
            this._printDialog = new System.Windows.Forms.PrintDialog();
            this._printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            sep3 = new System.Windows.Forms.ToolStripSeparator();
            _editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sep4 = new System.Windows.Forms.ToolStripSeparator();
            sep5 = new System.Windows.Forms.ToolStripSeparator();
            _stylesPanel = new System.Windows.Forms.Panel();
            _seedLbl = new System.Windows.Forms.Label();
            _stylesPanel.SuspendLayout();
            this._fillStylesGroup.SuspendLayout();
            this._lineStyleGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lineWidthTrack)).BeginInit();
            this._greedyStyleManagerPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamCTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamBTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamATrack)).BeginInit();
            this._randomStyleMangerPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._seedNum)).BeginInit();
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
            // _editMenuItem
            //
            _editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._undoMenuItem,
            this._redoMenuItem});
            _editMenuItem.Name = "_editMenuItem";
            _editMenuItem.Size = new System.Drawing.Size(39, 20);
            _editMenuItem.Text = "&Edit";
            _editMenuItem.Visible = false;
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
            // _stylesPanel
            //
            _stylesPanel.Controls.Add(this._fillStylesGroup);
            _stylesPanel.Controls.Add(this._lineStyleGroup);
            _stylesPanel.Controls.Add(this._greedyStyleManagerPnl);
            _stylesPanel.Controls.Add(this._randomStyleMangerPnl);
            _stylesPanel.Controls.Add(this._styleManagerTypeCmb);
            _stylesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            _stylesPanel.Location = new System.Drawing.Point(789, 49);
            _stylesPanel.Name = "_stylesPanel";
            _stylesPanel.Size = new System.Drawing.Size(195, 565);
            _stylesPanel.TabIndex = 6;
            _stylesPanel.Visible = true;
            //
            // _fillStylesGroup
            //
            this._fillStylesGroup.Controls.Add(this._styleList);
            this._fillStylesGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fillStylesGroup.Enabled = false;
            this._fillStylesGroup.Location = new System.Drawing.Point(0, 257);
            this._fillStylesGroup.Name = "_fillStylesGroup";
            this._fillStylesGroup.Size = new System.Drawing.Size(195, 308);
            this._fillStylesGroup.TabIndex = 5;
            this._fillStylesGroup.TabStop = false;
            this._fillStylesGroup.Text = "Fill styles";
            //
            // _styleList
            //
            this._styleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._styleList.Location = new System.Drawing.Point(3, 16);
            this._styleList.Name = "_styleList";
            this._styleList.Size = new System.Drawing.Size(189, 289);
            this._styleList.TabIndex = 3;
            this._styleList.StylesChanged += new System.EventHandler(this._styleList_StylesChanged);
            //
            // _lineStyleGroup
            //
            this._lineStyleGroup.Controls.Add(this._lineStyleControl);
            this._lineStyleGroup.Controls.Add(this._lineWidthTrack);
            this._lineStyleGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this._lineStyleGroup.Enabled = false;
            this._lineStyleGroup.Location = new System.Drawing.Point(0, 149);
            this._lineStyleGroup.Name = "_lineStyleGroup";
            this._lineStyleGroup.Size = new System.Drawing.Size(195, 108);
            this._lineStyleGroup.TabIndex = 4;
            this._lineStyleGroup.TabStop = false;
            this._lineStyleGroup.Text = "Line style";
            //
            // _lineStyleControl
            //
            this._lineStyleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lineStyleControl.Location = new System.Drawing.Point(3, 49);
            this._lineStyleControl.Name = "_lineStyleControl";
            this._lineStyleControl.Size = new System.Drawing.Size(189, 56);
            this._lineStyleControl.Style = null;
            this._lineStyleControl.TabIndex = 1;
            this._lineStyleControl.StyleChanged += new System.EventHandler(this._lineStyleControl_StyleChanged);
            //
            // _lineWidthTrack
            //
            this._lineWidthTrack.AutoSize = false;
            this._lineWidthTrack.Dock = System.Windows.Forms.DockStyle.Top;
            this._lineWidthTrack.Location = new System.Drawing.Point(3, 16);
            this._lineWidthTrack.Maximum = 100;
            this._lineWidthTrack.Name = "_lineWidthTrack";
            this._lineWidthTrack.Size = new System.Drawing.Size(189, 33);
            this._lineWidthTrack.TabIndex = 0;
            this._lineWidthTrack.TickFrequency = 10;
            this._toolTip.SetToolTip(this._lineWidthTrack, "The width of the line");
            this._lineWidthTrack.Value = 10;
            this._lineWidthTrack.Scroll += new System.EventHandler(this._lineWidthTrack_ValueChanged);
            this._lineWidthTrack.ValueChanged += new System.EventHandler(this._lineWidthTrack_ValueChanged);
            //
            // _greedyStyleManagerPnl
            //
            this._greedyStyleManagerPnl.Controls.Add(this._greedyParamCTrack);
            this._greedyStyleManagerPnl.Controls.Add(this._greedyParamBTrack);
            this._greedyStyleManagerPnl.Controls.Add(this._greedyParamATrack);
            this._greedyStyleManagerPnl.Dock = System.Windows.Forms.DockStyle.Top;
            this._greedyStyleManagerPnl.Location = new System.Drawing.Point(0, 47);
            this._greedyStyleManagerPnl.Name = "_greedyStyleManagerPnl";
            this._greedyStyleManagerPnl.Size = new System.Drawing.Size(195, 102);
            this._greedyStyleManagerPnl.TabIndex = 2;
            this._greedyStyleManagerPnl.Visible = false;
            this._greedyStyleManagerPnl.VisibleChanged += new System.EventHandler(this.StyleManagerPanel_VisibleChanged);
            //
            // _greedyParamCTrack
            //
            this._greedyParamCTrack.AutoSize = false;
            this._greedyParamCTrack.Dock = System.Windows.Forms.DockStyle.Top;
            this._greedyParamCTrack.Location = new System.Drawing.Point(0, 66);
            this._greedyParamCTrack.Name = "_greedyParamCTrack";
            this._greedyParamCTrack.Size = new System.Drawing.Size(195, 33);
            this._greedyParamCTrack.TabIndex = 2;
            this._greedyParamCTrack.ValueChanged += new System.EventHandler(this._greedyParamCTrack_ValueChanged);
            //
            // _greedyParamBTrack
            //
            this._greedyParamBTrack.AutoSize = false;
            this._greedyParamBTrack.Dock = System.Windows.Forms.DockStyle.Top;
            this._greedyParamBTrack.Location = new System.Drawing.Point(0, 33);
            this._greedyParamBTrack.Name = "_greedyParamBTrack";
            this._greedyParamBTrack.Size = new System.Drawing.Size(195, 33);
            this._greedyParamBTrack.TabIndex = 1;
            this._greedyParamBTrack.ValueChanged += new System.EventHandler(this._greedyParamBTrack_ValueChanged);
            //
            // _greedyParamATrack
            //
            this._greedyParamATrack.AutoSize = false;
            this._greedyParamATrack.Dock = System.Windows.Forms.DockStyle.Top;
            this._greedyParamATrack.Location = new System.Drawing.Point(0, 0);
            this._greedyParamATrack.Name = "_greedyParamATrack";
            this._greedyParamATrack.Size = new System.Drawing.Size(195, 33);
            this._greedyParamATrack.TabIndex = 0;
            this._greedyParamATrack.ValueChanged += new System.EventHandler(this._greedyParamATrack_ValueChanged);
            //
            // _randomStyleMangerPnl
            //
            this._randomStyleMangerPnl.Controls.Add(this._randomSeedBtn);
            this._randomStyleMangerPnl.Controls.Add(this._seedNum);
            this._randomStyleMangerPnl.Controls.Add(_seedLbl);
            this._randomStyleMangerPnl.Dock = System.Windows.Forms.DockStyle.Top;
            this._randomStyleMangerPnl.Location = new System.Drawing.Point(0, 21);
            this._randomStyleMangerPnl.Name = "_randomStyleMangerPnl";
            this._randomStyleMangerPnl.Size = new System.Drawing.Size(195, 26);
            this._randomStyleMangerPnl.TabIndex = 1;
            this._randomStyleMangerPnl.Visible = false;
            this._randomStyleMangerPnl.VisibleChanged += new System.EventHandler(this.StyleManagerPanel_VisibleChanged);
            //
            // _randomSeedBtn
            //
            this._randomSeedBtn.Location = new System.Drawing.Point(136, 2);
            this._randomSeedBtn.Name = "_randomSeedBtn";
            this._randomSeedBtn.Size = new System.Drawing.Size(56, 22);
            this._randomSeedBtn.TabIndex = 2;
            this._randomSeedBtn.Text = "&Random";
            this._toolTip.SetToolTip(this._randomSeedBtn, "Picks a random seed");
            this._randomSeedBtn.UseVisualStyleBackColor = true;
            this._randomSeedBtn.Click += new System.EventHandler(this._randomSeedBtn_Click);
            //
            // _seedNum
            //
            this._seedNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._seedNum.Location = new System.Drawing.Point(44, 3);
            this._seedNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this._seedNum.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this._seedNum.Name = "_seedNum";
            this._seedNum.Size = new System.Drawing.Size(86, 20);
            this._seedNum.TabIndex = 1;
            this._seedNum.ValueChanged += new System.EventHandler(this._seedNum_ValueChanged);
            //
            // _seedLbl
            //
            _seedLbl.AutoSize = true;
            _seedLbl.Location = new System.Drawing.Point(3, 6);
            _seedLbl.Name = "_seedLbl";
            _seedLbl.Size = new System.Drawing.Size(35, 13);
            _seedLbl.TabIndex = 0;
            _seedLbl.Text = "Seed:";
            //
            // _styleManagerTypeCmb
            //
            this._styleManagerTypeCmb.Dock = System.Windows.Forms.DockStyle.Top;
            this._styleManagerTypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._styleManagerTypeCmb.Enabled = false;
            this._styleManagerTypeCmb.FormattingEnabled = true;
            this._styleManagerTypeCmb.Location = new System.Drawing.Point(0, 0);
            this._styleManagerTypeCmb.Name = "_styleManagerTypeCmb";
            this._styleManagerTypeCmb.Size = new System.Drawing.Size(195, 21);
            this._styleManagerTypeCmb.TabIndex = 0;
            this._styleManagerTypeCmb.SelectedIndexChanged += new System.EventHandler(this._styleManagerTypeCmb_SelectedIndexChanged);
            //
            // _menuStrip
            //
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            _editMenuItem,
            this._toolsMenuItem,
            this._helpMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(984, 24);
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
            this._pageSetupMenuItem,
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
            this._newMenuItem.Image = global::EscherTiler.Properties.Resources.NewIcon;
            this._newMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newMenuItem.Name = "_newMenuItem";
            this._newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._newMenuItem.Size = new System.Drawing.Size(146, 22);
            this._newMenuItem.Text = "&New";
            this._newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            //
            // _openMenuItem
            //
            this._openMenuItem.Image = global::EscherTiler.Properties.Resources.OpenIcon;
            this._openMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openMenuItem.Name = "_openMenuItem";
            this._openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openMenuItem.Size = new System.Drawing.Size(146, 22);
            this._openMenuItem.Text = "&Open";
            this._openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            //
            // _saveMenuItem
            //
            this._saveMenuItem.Image = global::EscherTiler.Properties.Resources.SaveIcon;
            this._saveMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveMenuItem.Name = "_saveMenuItem";
            this._saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveMenuItem.Size = new System.Drawing.Size(146, 22);
            this._saveMenuItem.Text = "&Save";
            this._saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            //
            // _saveAsMenuItem
            //
            this._saveAsMenuItem.Name = "_saveAsMenuItem";
            this._saveAsMenuItem.Size = new System.Drawing.Size(146, 22);
            this._saveAsMenuItem.Text = "Save &As";
            this._saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            //
            // _pageSetupMenuItem
            //
            this._pageSetupMenuItem.Name = "_pageSetupMenuItem";
            this._pageSetupMenuItem.Size = new System.Drawing.Size(146, 22);
            this._pageSetupMenuItem.Text = "Page Set&up";
            this._pageSetupMenuItem.Click += new System.EventHandler(this._pageSetupMenuItem_Click);
            //
            // _printMenuItem
            //
            this._printMenuItem.Image = global::EscherTiler.Properties.Resources.PrintIcon;
            this._printMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printMenuItem.Name = "_printMenuItem";
            this._printMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this._printMenuItem.Size = new System.Drawing.Size(146, 22);
            this._printMenuItem.Text = "&Print";
            this._printMenuItem.Click += new System.EventHandler(this.printMenuItem_Click);
            //
            // _printPreviewMenuItem
            //
            this._printPreviewMenuItem.Image = global::EscherTiler.Properties.Resources.PrintPreviewIcon;
            this._printPreviewMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printPreviewMenuItem.Name = "_printPreviewMenuItem";
            this._printPreviewMenuItem.Size = new System.Drawing.Size(146, 22);
            this._printPreviewMenuItem.Text = "Print Pre&view";
            this._printPreviewMenuItem.Click += new System.EventHandler(this.printPreviewMenuItem_Click);
            //
            // _exitMenuItem
            //
            this._exitMenuItem.Name = "_exitMenuItem";
            this._exitMenuItem.Size = new System.Drawing.Size(146, 22);
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
            this._toolStrip.Size = new System.Drawing.Size(984, 25);
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "Tools";
            //
            // _newButton
            //
            this._newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newButton.Image = global::EscherTiler.Properties.Resources.NewIcon;
            this._newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newButton.Name = "_newButton";
            this._newButton.Size = new System.Drawing.Size(23, 22);
            this._newButton.Text = "&New";
            this._newButton.Click += new System.EventHandler(this.newButton_Click);
            //
            // _openButton
            //
            this._openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openButton.Image = global::EscherTiler.Properties.Resources.OpenIcon;
            this._openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(23, 22);
            this._openButton.Text = "&Open";
            this._openButton.Click += new System.EventHandler(this.openButton_Click);
            //
            // _saveButton
            //
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveButton.Image = global::EscherTiler.Properties.Resources.SaveIcon;
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(23, 22);
            this._saveButton.Text = "&Save";
            this._saveButton.Click += new System.EventHandler(this.saveButton_Click);
            //
            // _printButton
            //
            this._printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._printButton.Image = global::EscherTiler.Properties.Resources.PrintIcon;
            this._printButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._printButton.Name = "_printButton";
            this._printButton.Size = new System.Drawing.Size(23, 22);
            this._printButton.Text = "&Print";
            this._printButton.Click += new System.EventHandler(this.printButton_Click);
            //
            // _helpButton
            //
            this._helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._helpButton.Image = global::EscherTiler.Properties.Resources.HelpIcon;
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
            this._operationToolStrip.Size = new System.Drawing.Size(41, 590);
            this._operationToolStrip.TabIndex = 2;
            this._operationToolStrip.Text = "Operations";
            //
            // _panToolBtn
            //
            this._panToolBtn.AutoSize = false;
            this._panToolBtn.Checked = true;
            this._panToolBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this._panToolBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._panToolBtn.Image = global::EscherTiler.Properties.Resources.PanTool_Icon;
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
            this._statusStrip.Location = new System.Drawing.Point(0, 639);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(984, 22);
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
            this._zoomText,
            this._changeLineTypeCmb});
            this._contextToolStrip.Location = new System.Drawing.Point(41, 614);
            this._contextToolStrip.Name = "_contextToolStrip";
            this._contextToolStrip.Size = new System.Drawing.Size(943, 25);
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
            // _changeLineTypeCmb
            //
            this._changeLineTypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._changeLineTypeCmb.Name = "_changeLineTypeCmb";
            this._changeLineTypeCmb.Size = new System.Drawing.Size(121, 25);
            this._changeLineTypeCmb.Visible = false;
            this._changeLineTypeCmb.SelectedIndexChanged += new System.EventHandler(this.changeLineTypeCmb_SelectedIndexChanged);
            //
            // _saveFileDialog
            //
            this._saveFileDialog.DefaultExt = "esch";
            this._saveFileDialog.Filter = "Tiling|*.esch";
            //
            // _openFileDialog
            //
            this._openFileDialog.DefaultExt = "esch";
            this._openFileDialog.Filter = "Tiling|*.esch";
            //
            // _renderControl
            //
            this._renderControl.Cursor = System.Windows.Forms.Cursors.Cross;
            this._renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._renderControl.Location = new System.Drawing.Point(41, 49);
            this._renderControl.Name = "_renderControl";
            this._renderControl.Size = new System.Drawing.Size(748, 565);
            this._renderControl.TabIndex = 5;
            this._renderControl.Render += new EscherTiler.RenderDelegate(this.renderControl_Render);
            this._renderControl.Layout += new System.Windows.Forms.LayoutEventHandler(this.renderControl_Layout);
            this._renderControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseDown);
            this._renderControl.MouseLeave += new System.EventHandler(this.renderControl_MouseLeave);
            this._renderControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseMove);
            this._renderControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseUp);
            //
            // _pageSetupDialog
            //
            this._pageSetupDialog.Document = this._printDocument;
            this._pageSetupDialog.EnableMetric = true;
            //
            // _printDocument
            //
            this._printDocument.DocumentName = "Tiling";
            this._printDocument.PrintMode = EscherTiler.Graphics.GDI.TilingPrintMode.TilingFull;
            this._printDocument.Tile = null;
            this._printDocument.Tiling = null;
            //
            // _printDialog
            //
            this._printDialog.Document = this._printDocument;
            this._printDialog.UseEXDialog = true;
            //
            // _printPreviewDialog
            //
            this._printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this._printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this._printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this._printPreviewDialog.Document = this._printDocument;
            this._printPreviewDialog.Enabled = true;
            this._printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("_printPreviewDialog.Icon")));
            this._printPreviewDialog.Name = "_printPreviewDialog";
            this._printPreviewDialog.UseAntiAlias = true;
            this._printPreviewDialog.Visible = false;
            //
            // Main
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this._renderControl);
            this.Controls.Add(_stylesPanel);
            this.Controls.Add(this._contextToolStrip);
            this.Controls.Add(this._operationToolStrip);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._menuStrip);
            this.Controls.Add(this._statusStrip);
            this.MainMenuStrip = this._menuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Escher Tiler";
            this.Click += new System.EventHandler(this.toolBtn_Click);
            _stylesPanel.ResumeLayout(false);
            this._fillStylesGroup.ResumeLayout(false);
            this._lineStyleGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._lineWidthTrack)).EndInit();
            this._greedyStyleManagerPnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamCTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamBTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._greedyParamATrack)).EndInit();
            this._randomStyleMangerPnl.ResumeLayout(false);
            this._randomStyleMangerPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._seedNum)).EndInit();
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

        [NotNull]
        private MenuStrip _menuStrip;

        [NotNull]
        private ToolStripMenuItem _fileMenuItem;

        [NotNull]
        private ToolStripMenuItem _newMenuItem;

        [NotNull]
        private ToolStripMenuItem _openMenuItem;

        [NotNull]
        private ToolStripMenuItem _saveMenuItem;

        [NotNull]
        private ToolStripMenuItem _saveAsMenuItem;

        [NotNull]
        private ToolStripMenuItem _printMenuItem;

        [NotNull]
        private ToolStripMenuItem _printPreviewMenuItem;

        [NotNull]
        private ToolStripMenuItem _exitMenuItem;

        [NotNull]
        private ToolStripMenuItem _undoMenuItem;

        [NotNull]
        private ToolStripMenuItem _redoMenuItem;

        [NotNull]
        private ToolStripMenuItem _toolsMenuItem;

        [NotNull]
        private ToolStripMenuItem _customizeMenuItem;

        [NotNull]
        private ToolStripMenuItem _optionsMenuItem;

        [NotNull]
        private ToolStripMenuItem _helpMenuItem;

        [NotNull]
        private ToolStripMenuItem _indexMenuItem;

        [NotNull]
        private ToolStripMenuItem _searchMenuItem;

        [NotNull]
        private ToolStripMenuItem _aboutMenuItem;

        [NotNull]
        private ToolStripButton _newButton;

        [NotNull]
        private ToolStripButton _openButton;

        [NotNull]
        private ToolStripButton _saveButton;

        [NotNull]
        private ToolStripButton _printButton;

        [NotNull]
        private ToolStripButton _helpButton;

        [NotNull]
        private ToolStrip _toolStrip;

        [NotNull]
        private ToolStrip _operationToolStrip;

        [NotNull]
        private ToolStripButton _panToolBtn;

        [NotNull]
        private StatusStrip _statusStrip;

        [NotNull]
        private ToolStripStatusLabel _statusInfoLabel;

        [NotNull]
        private ToolStrip _contextToolStrip;

        [NotNull]
        private RenderControl _renderControl;

        [NotNull]
        private ToolStripTextBox _zoomText;

        [NotNull]
        private ToolStripComboBox _changeLineTypeCmb;

        [NotNull]
        private ToolStripMenuItem _pageSetupMenuItem;

        [NotNull]
        private PageSetupDialog _pageSetupDialog;

        [NotNull]
        private PrintDialog _printDialog;

        [NotNull]
        private PrintPreviewDialog _printPreviewDialog;

        [NotNull]
        private Graphics.GDI.TilerPrintDocument _printDocument;

        [NotNull]
        private SaveFileDialog _saveFileDialog;

        [NotNull]
        private OpenFileDialog _openFileDialog;

        [NotNull]
        private ComboBox _styleManagerTypeCmb;

        [NotNull]
        private Panel _randomStyleMangerPnl;

        [NotNull]
        private Button _randomSeedBtn;

        [NotNull]
        private ToolTip _toolTip;

        [NotNull]
        private NumericUpDown _seedNum;

        [NotNull]
        private Panel _greedyStyleManagerPnl;

        [NotNull]
        private TrackBar _greedyParamCTrack;

        [NotNull]
        private TrackBar _greedyParamBTrack;

        [NotNull]
        private TrackBar _greedyParamATrack;

        [NotNull]
        private StyleListView _styleList;

        [NotNull]
        private StyleControl _lineStyleControl;

        [NotNull]
        private TrackBar _lineWidthTrack;

        [NotNull]
        private GroupBox _fillStylesGroup;

        [NotNull]
        private GroupBox _lineStyleGroup;

        #endregion
    }
}

