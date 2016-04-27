using JetBrains.Annotations;

namespace EscherTiler
{
    partial class TilingPrintSettingsDialog
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
            System.Windows.Forms.GroupBox printModeGroup;
            this.selectTileBtn = new System.Windows.Forms.Button();
            this.tilingFullBtn = new System.Windows.Forms.RadioButton();
            this.tilingLinesBtn = new System.Windows.Forms.RadioButton();
            this.tileFullBtn = new System.Windows.Forms.RadioButton();
            this.tileLinesBtn = new System.Windows.Forms.RadioButton();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            printModeGroup = new System.Windows.Forms.GroupBox();
            printModeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // printModeGroup
            // 
            printModeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            printModeGroup.Controls.Add(this.selectTileBtn);
            printModeGroup.Controls.Add(this.tilingFullBtn);
            printModeGroup.Controls.Add(this.tilingLinesBtn);
            printModeGroup.Controls.Add(this.tileFullBtn);
            printModeGroup.Controls.Add(this.tileLinesBtn);
            printModeGroup.Location = new System.Drawing.Point(12, 12);
            printModeGroup.Name = "printModeGroup";
            printModeGroup.Size = new System.Drawing.Size(461, 111);
            printModeGroup.TabIndex = 6;
            printModeGroup.TabStop = false;
            printModeGroup.Text = "Print mode";
            // 
            // selectTileBtn
            // 
            this.selectTileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectTileBtn.Location = new System.Drawing.Point(380, 82);
            this.selectTileBtn.Name = "selectTileBtn";
            this.selectTileBtn.Size = new System.Drawing.Size(75, 23);
            this.selectTileBtn.TabIndex = 7;
            this.selectTileBtn.Text = "&Select tile...";
            this.selectTileBtn.UseVisualStyleBackColor = true;
            this.selectTileBtn.Click += new System.EventHandler(this.selectTileBtn_Click);
            // 
            // tilingFullBtn
            // 
            this.tilingFullBtn.AutoSize = true;
            this.tilingFullBtn.Location = new System.Drawing.Point(6, 19);
            this.tilingFullBtn.Name = "tilingFullBtn";
            this.tilingFullBtn.Size = new System.Drawing.Size(116, 17);
            this.tilingFullBtn.TabIndex = 0;
            this.tilingFullBtn.TabStop = true;
            this.tilingFullBtn.Text = "Full tiling with styles";
            this.tilingFullBtn.UseVisualStyleBackColor = true;
            this.tilingFullBtn.CheckedChanged += new System.EventHandler(this.modeBtn_CheckedChanged);
            // 
            // tilingLinesBtn
            // 
            this.tilingLinesBtn.AutoSize = true;
            this.tilingLinesBtn.Location = new System.Drawing.Point(6, 42);
            this.tilingLinesBtn.Name = "tilingLinesBtn";
            this.tilingLinesBtn.Size = new System.Drawing.Size(148, 17);
            this.tilingLinesBtn.TabIndex = 1;
            this.tilingLinesBtn.TabStop = true;
            this.tilingLinesBtn.Text = "Full tiling with outlines only";
            this.tilingLinesBtn.UseVisualStyleBackColor = true;
            this.tilingLinesBtn.CheckedChanged += new System.EventHandler(this.modeBtn_CheckedChanged);
            // 
            // tileFullBtn
            // 
            this.tileFullBtn.AutoSize = true;
            this.tileFullBtn.Location = new System.Drawing.Point(6, 65);
            this.tileFullBtn.Name = "tileFullBtn";
            this.tileFullBtn.Size = new System.Drawing.Size(116, 17);
            this.tileFullBtn.TabIndex = 2;
            this.tileFullBtn.TabStop = true;
            this.tileFullBtn.Text = "Single tile with style";
            this.tileFullBtn.UseVisualStyleBackColor = true;
            this.tileFullBtn.CheckedChanged += new System.EventHandler(this.modeBtn_CheckedChanged);
            // 
            // tileLinesBtn
            // 
            this.tileLinesBtn.AutoSize = true;
            this.tileLinesBtn.Location = new System.Drawing.Point(6, 88);
            this.tileLinesBtn.Name = "tileLinesBtn";
            this.tileLinesBtn.Size = new System.Drawing.Size(148, 17);
            this.tileLinesBtn.TabIndex = 3;
            this.tileLinesBtn.TabStop = true;
            this.tileLinesBtn.Text = "Single tile with outline only";
            this.tileLinesBtn.UseVisualStyleBackColor = true;
            this.tileLinesBtn.CheckedChanged += new System.EventHandler(this.modeBtn_CheckedChanged);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(398, 129);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "&OK";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(317, 129);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // TilingPrintSettingsDialog
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(485, 164);
            this.Controls.Add(printModeGroup);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TilingPrintSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tiling print settings";
            this.TopMost = true;
            printModeGroup.ResumeLayout(false);
            printModeGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        [NotNull]
        private System.Windows.Forms.RadioButton tilingFullBtn;

        [NotNull]
        private System.Windows.Forms.RadioButton tilingLinesBtn;

        [NotNull]
        private System.Windows.Forms.RadioButton tileFullBtn;

        [NotNull]
        private System.Windows.Forms.RadioButton tileLinesBtn;

        [NotNull]
        private System.Windows.Forms.Button okBtn;

        [NotNull]
        private System.Windows.Forms.Button cancelBtn;

        [NotNull]
        private System.Windows.Forms.Button selectTileBtn;

        #endregion
    }
}