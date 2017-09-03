namespace YoutubeDown
{
    partial class SettingsWindow
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
            this.labelFFmpegLocation = new System.Windows.Forms.Label();
            this.textBoxFFmpegLocation = new System.Windows.Forms.TextBox();
            this.buttonFFmpegLocation = new System.Windows.Forms.Button();
            this.buttonDownloadLocation = new System.Windows.Forms.Button();
            this.textBoxDownloadLocation = new System.Windows.Forms.TextBox();
            this.labelDownloadLocation = new System.Windows.Forms.Label();
            this.checkBoxOverwriteFiles = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFFmpegLocation
            // 
            this.labelFFmpegLocation.AutoSize = true;
            this.labelFFmpegLocation.Location = new System.Drawing.Point(12, 9);
            this.labelFFmpegLocation.Name = "labelFFmpegLocation";
            this.labelFFmpegLocation.Size = new System.Drawing.Size(79, 13);
            this.labelFFmpegLocation.TabIndex = 0;
            this.labelFFmpegLocation.Text = "ffmpeg location";
            // 
            // textBoxFFmpegLocation
            // 
            this.textBoxFFmpegLocation.Location = new System.Drawing.Point(15, 25);
            this.textBoxFFmpegLocation.Name = "textBoxFFmpegLocation";
            this.textBoxFFmpegLocation.Size = new System.Drawing.Size(226, 20);
            this.textBoxFFmpegLocation.TabIndex = 1;
            // 
            // buttonFFmpegLocation
            // 
            this.buttonFFmpegLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFFmpegLocation.Location = new System.Drawing.Point(247, 25);
            this.buttonFFmpegLocation.Name = "buttonFFmpegLocation";
            this.buttonFFmpegLocation.Size = new System.Drawing.Size(25, 20);
            this.buttonFFmpegLocation.TabIndex = 2;
            this.buttonFFmpegLocation.Text = "..";
            this.buttonFFmpegLocation.UseVisualStyleBackColor = true;
            this.buttonFFmpegLocation.Click += new System.EventHandler(this.buttonFFmpegLocation_Click);
            // 
            // buttonDownloadLocation
            // 
            this.buttonDownloadLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadLocation.Location = new System.Drawing.Point(247, 64);
            this.buttonDownloadLocation.Name = "buttonDownloadLocation";
            this.buttonDownloadLocation.Size = new System.Drawing.Size(25, 20);
            this.buttonDownloadLocation.TabIndex = 5;
            this.buttonDownloadLocation.Text = "..";
            this.buttonDownloadLocation.UseVisualStyleBackColor = true;
            this.buttonDownloadLocation.Click += new System.EventHandler(this.buttonDownloadLocation_Click);
            // 
            // textBoxDownloadLocation
            // 
            this.textBoxDownloadLocation.Location = new System.Drawing.Point(15, 64);
            this.textBoxDownloadLocation.Name = "textBoxDownloadLocation";
            this.textBoxDownloadLocation.Size = new System.Drawing.Size(226, 20);
            this.textBoxDownloadLocation.TabIndex = 4;
            // 
            // labelDownloadLocation
            // 
            this.labelDownloadLocation.AutoSize = true;
            this.labelDownloadLocation.Location = new System.Drawing.Point(12, 48);
            this.labelDownloadLocation.Name = "labelDownloadLocation";
            this.labelDownloadLocation.Size = new System.Drawing.Size(93, 13);
            this.labelDownloadLocation.TabIndex = 3;
            this.labelDownloadLocation.Text = "download location";
            // 
            // checkBoxOverwriteFiles
            // 
            this.checkBoxOverwriteFiles.AutoSize = true;
            this.checkBoxOverwriteFiles.Location = new System.Drawing.Point(15, 90);
            this.checkBoxOverwriteFiles.Name = "checkBoxOverwriteFiles";
            this.checkBoxOverwriteFiles.Size = new System.Drawing.Size(90, 17);
            this.checkBoxOverwriteFiles.TabIndex = 6;
            this.checkBoxOverwriteFiles.Text = "overwrite files";
            this.checkBoxOverwriteFiles.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Location = new System.Drawing.Point(12, 113);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(197, 113);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 145);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkBoxOverwriteFiles);
            this.Controls.Add(this.buttonDownloadLocation);
            this.Controls.Add(this.textBoxDownloadLocation);
            this.Controls.Add(this.labelDownloadLocation);
            this.Controls.Add(this.buttonFFmpegLocation);
            this.Controls.Add(this.textBoxFFmpegLocation);
            this.Controls.Add(this.labelFFmpegLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFFmpegLocation;
        private System.Windows.Forms.TextBox textBoxFFmpegLocation;
        private System.Windows.Forms.Button buttonFFmpegLocation;
        private System.Windows.Forms.Button buttonDownloadLocation;
        private System.Windows.Forms.TextBox textBoxDownloadLocation;
        private System.Windows.Forms.Label labelDownloadLocation;
        private System.Windows.Forms.CheckBox checkBoxOverwriteFiles;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
    }
}