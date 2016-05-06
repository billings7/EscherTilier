using JetBrains.Annotations;

namespace EscherTiler
{
    partial class StyleControl
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
            this._styleTypeCmb = new System.Windows.Forms.ComboBox();
            this._previewPnl = new System.Windows.Forms.Panel();
            this._colourDialog = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // _styleTypeCmb
            // 
            this._styleTypeCmb.Dock = System.Windows.Forms.DockStyle.Top;
            this._styleTypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._styleTypeCmb.FormattingEnabled = true;
            this._styleTypeCmb.Location = new System.Drawing.Point(0, 0);
            this._styleTypeCmb.Name = "_styleTypeCmb";
            this._styleTypeCmb.Size = new System.Drawing.Size(256, 21);
            this._styleTypeCmb.TabIndex = 0;
            this._styleTypeCmb.Visible = false;
            // 
            // _previewPnl
            // 
            this._previewPnl.Dock = System.Windows.Forms.DockStyle.Top;
            this._previewPnl.Location = new System.Drawing.Point(0, 21);
            this._previewPnl.Name = "_previewPnl";
            this._previewPnl.Size = new System.Drawing.Size(256, 47);
            this._previewPnl.TabIndex = 1;
            this._previewPnl.Paint += new System.Windows.Forms.PaintEventHandler(this._previewPnl_Paint);
            this._previewPnl.DoubleClick += new System.EventHandler(this._previewPnl_DoubleClick);
            // 
            // StyleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._previewPnl);
            this.Controls.Add(this._styleTypeCmb);
            this.Name = "StyleControl";
            this.Size = new System.Drawing.Size(256, 68);
            this.ResumeLayout(false);

        }

        [NotNull]
        private System.Windows.Forms.ComboBox _styleTypeCmb;

        [NotNull]
        private System.Windows.Forms.Panel _previewPnl;

        #endregion

        private System.Windows.Forms.ColorDialog _colourDialog;
    }
}
