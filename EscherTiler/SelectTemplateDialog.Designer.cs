using JetBrains.Annotations;

namespace EscherTiler
{
    partial class SelectTemplateDialog
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
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._templateList = new System.Windows.Forms.ListView();
            this._thumbnailList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBtn.Enabled = false;
            this._okBtn.Location = new System.Drawing.Point(693, 525);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(75, 23);
            this._okBtn.TabIndex = 0;
            this._okBtn.Text = "&OK";
            this._okBtn.UseVisualStyleBackColor = true;
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(774, 525);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 1;
            this._cancelBtn.Text = "&Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _templateList
            // 
            this._templateList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._templateList.LargeImageList = this._thumbnailList;
            this._templateList.Location = new System.Drawing.Point(0, 0);
            this._templateList.Name = "_templateList";
            this._templateList.Size = new System.Drawing.Size(861, 519);
            this._templateList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._templateList.TabIndex = 2;
            this._templateList.UseCompatibleStateImageBehavior = false;
            this._templateList.SelectedIndexChanged += new System.EventHandler(this._templateList_SelectedIndexChanged);
            // 
            // _thumbnailList
            // 
            this._thumbnailList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._thumbnailList.ImageSize = new System.Drawing.Size(128, 128);
            this._thumbnailList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SelectTemplateDialog
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 560);
            this.Controls.Add(this._templateList);
            this.Controls.Add(this._cancelBtn);
            this.Controls.Add(this._okBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTemplateDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Template...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        [NotNull]
        private System.Windows.Forms.Button _okBtn;
        [NotNull]
        private System.Windows.Forms.Button _cancelBtn;
        [NotNull]
        private System.Windows.Forms.ListView _templateList;
        [NotNull]
        private System.Windows.Forms.ImageList _thumbnailList;

        #endregion
    }
}