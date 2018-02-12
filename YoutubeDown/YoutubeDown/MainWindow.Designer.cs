namespace YoutubeDown
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxVideoId = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelVideoId = new System.Windows.Forms.Label();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelVideoTitel = new System.Windows.Forms.Label();
            this.labelVideoTitelValue = new System.Windows.Forms.Label();
            this.labelAudioSizeValue = new System.Windows.Forms.Label();
            this.labelAudioSize = new System.Windows.Forms.Label();
            this.labelVideoSizeValue = new System.Windows.Forms.Label();
            this.labelVideoSize = new System.Windows.Forms.Label();
            this.labelTotalSizeValue = new System.Windows.Forms.Label();
            this.labelTotalSize = new System.Windows.Forms.Label();
            this.toolTipTitle = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonAddDownload = new System.Windows.Forms.Button();
            this.groupBoxAllVideos = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBoxSelected = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxAllVideos.SuspendLayout();
            this.groupBoxSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxVideoId
            // 
            this.textBoxVideoId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVideoId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVideoId.Location = new System.Drawing.Point(14, 219);
            this.textBoxVideoId.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxVideoId.Name = "textBoxVideoId";
            this.textBoxVideoId.Size = new System.Drawing.Size(598, 26);
            this.textBoxVideoId.TabIndex = 0;
            this.textBoxVideoId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxVideoId_KeyPress);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 646);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(665, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // labelVideoId
            // 
            this.labelVideoId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVideoId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVideoId.Location = new System.Drawing.Point(10, 191);
            this.labelVideoId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVideoId.Name = "labelVideoId";
            this.labelVideoId.Size = new System.Drawing.Size(331, 26);
            this.labelVideoId.TabIndex = 2;
            this.labelVideoId.Text = "Video Id/Url";
            this.labelVideoId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDownload.Location = new System.Drawing.Point(389, 143);
            this.buttonDownload.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(132, 64);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(665, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // labelVideoTitel
            // 
            this.labelVideoTitel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVideoTitel.Location = new System.Drawing.Point(11, 24);
            this.labelVideoTitel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVideoTitel.Name = "labelVideoTitel";
            this.labelVideoTitel.Size = new System.Drawing.Size(224, 26);
            this.labelVideoTitel.TabIndex = 4;
            this.labelVideoTitel.Text = "Titel";
            this.labelVideoTitel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVideoTitelValue
            // 
            this.labelVideoTitelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVideoTitelValue.Location = new System.Drawing.Point(11, 50);
            this.labelVideoTitelValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.labelVideoTitelValue.Name = "labelVideoTitelValue";
            this.labelVideoTitelValue.Size = new System.Drawing.Size(224, 26);
            this.labelVideoTitelValue.TabIndex = 5;
            this.labelVideoTitelValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAudioSizeValue
            // 
            this.labelAudioSizeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAudioSizeValue.Location = new System.Drawing.Point(11, 104);
            this.labelAudioSizeValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.labelAudioSizeValue.Name = "labelAudioSizeValue";
            this.labelAudioSizeValue.Size = new System.Drawing.Size(224, 26);
            this.labelAudioSizeValue.TabIndex = 7;
            this.labelAudioSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAudioSize
            // 
            this.labelAudioSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAudioSize.Location = new System.Drawing.Point(11, 78);
            this.labelAudioSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAudioSize.Name = "labelAudioSize";
            this.labelAudioSize.Size = new System.Drawing.Size(224, 26);
            this.labelAudioSize.TabIndex = 6;
            this.labelAudioSize.Text = "Audio Size";
            this.labelAudioSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVideoSizeValue
            // 
            this.labelVideoSizeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVideoSizeValue.Location = new System.Drawing.Point(173, 50);
            this.labelVideoSizeValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.labelVideoSizeValue.Name = "labelVideoSizeValue";
            this.labelVideoSizeValue.Size = new System.Drawing.Size(224, 26);
            this.labelVideoSizeValue.TabIndex = 9;
            this.labelVideoSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVideoSize
            // 
            this.labelVideoSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVideoSize.Location = new System.Drawing.Point(173, 24);
            this.labelVideoSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVideoSize.Name = "labelVideoSize";
            this.labelVideoSize.Size = new System.Drawing.Size(224, 26);
            this.labelVideoSize.TabIndex = 8;
            this.labelVideoSize.Text = "Video Size";
            this.labelVideoSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalSizeValue
            // 
            this.labelTotalSizeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalSizeValue.Location = new System.Drawing.Point(173, 104);
            this.labelTotalSizeValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.labelTotalSizeValue.Name = "labelTotalSizeValue";
            this.labelTotalSizeValue.Size = new System.Drawing.Size(224, 26);
            this.labelTotalSizeValue.TabIndex = 11;
            this.labelTotalSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalSize
            // 
            this.labelTotalSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalSize.Location = new System.Drawing.Point(173, 78);
            this.labelTotalSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalSize.Name = "labelTotalSize";
            this.labelTotalSize.Size = new System.Drawing.Size(224, 26);
            this.labelTotalSize.TabIndex = 10;
            this.labelTotalSize.Text = "Total Size";
            this.labelTotalSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 255);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(665, 391);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.VirtualMode = true;
            this.dataGridView1.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView1_CellValueNeeded);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // buttonAddDownload
            // 
            this.buttonAddDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddDownload.Image = global::YoutubeDown.Properties.Resources.plus;
            this.buttonAddDownload.Location = new System.Drawing.Point(617, 213);
            this.buttonAddDownload.Name = "buttonAddDownload";
            this.buttonAddDownload.Size = new System.Drawing.Size(36, 36);
            this.buttonAddDownload.TabIndex = 13;
            this.buttonAddDownload.UseVisualStyleBackColor = true;
            this.buttonAddDownload.Click += new System.EventHandler(this.buttonAddDownload_Click);
            // 
            // groupBoxAllVideos
            // 
            this.groupBoxAllVideos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAllVideos.Controls.Add(this.button5);
            this.groupBoxAllVideos.Controls.Add(this.button7);
            this.groupBoxAllVideos.Controls.Add(this.button6);
            this.groupBoxAllVideos.Location = new System.Drawing.Point(520, 143);
            this.groupBoxAllVideos.Name = "groupBoxAllVideos";
            this.groupBoxAllVideos.Size = new System.Drawing.Size(133, 64);
            this.groupBoxAllVideos.TabIndex = 17;
            this.groupBoxAllVideos.TabStop = false;
            this.groupBoxAllVideos.Text = "All Videos";
            // 
            // button5
            // 
            this.button5.Image = global::YoutubeDown.Properties.Resources.stop;
            this.button5.Location = new System.Drawing.Point(90, 19);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(36, 36);
            this.button5.TabIndex = 20;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button7.Image = global::YoutubeDown.Properties.Resources.play;
            this.button7.Location = new System.Drawing.Point(6, 19);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(36, 36);
            this.button7.TabIndex = 18;
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Image = global::YoutubeDown.Properties.Resources.pause;
            this.button6.Location = new System.Drawing.Point(48, 19);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(36, 36);
            this.button6.TabIndex = 19;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // groupBoxSelected
            // 
            this.groupBoxSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSelected.Controls.Add(this.button2);
            this.groupBoxSelected.Controls.Add(this.button3);
            this.groupBoxSelected.Controls.Add(this.button4);
            this.groupBoxSelected.Location = new System.Drawing.Point(520, 73);
            this.groupBoxSelected.Name = "groupBoxSelected";
            this.groupBoxSelected.Size = new System.Drawing.Size(133, 64);
            this.groupBoxSelected.TabIndex = 21;
            this.groupBoxSelected.TabStop = false;
            this.groupBoxSelected.Text = "Selected Videos";
            // 
            // button2
            // 
            this.button2.Image = global::YoutubeDown.Properties.Resources.stop;
            this.button2.Location = new System.Drawing.Point(90, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 36);
            this.button2.TabIndex = 20;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Image = global::YoutubeDown.Properties.Resources.play;
            this.button3.Location = new System.Drawing.Point(6, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 36);
            this.button3.TabIndex = 18;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Image = global::YoutubeDown.Properties.Resources.pause;
            this.button4.Location = new System.Drawing.Point(48, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(36, 36);
            this.button4.TabIndex = 19;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 668);
            this.Controls.Add(this.groupBoxSelected);
            this.Controls.Add(this.groupBoxAllVideos);
            this.Controls.Add(this.buttonAddDownload);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelTotalSizeValue);
            this.Controls.Add(this.labelTotalSize);
            this.Controls.Add(this.labelVideoSizeValue);
            this.Controls.Add(this.labelVideoSize);
            this.Controls.Add(this.labelAudioSizeValue);
            this.Controls.Add(this.labelAudioSize);
            this.Controls.Add(this.labelVideoTitelValue);
            this.Controls.Add(this.labelVideoTitel);
            this.Controls.Add(this.labelVideoId);
            this.Controls.Add(this.textBoxVideoId);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "YoutubeDown";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxAllVideos.ResumeLayout(false);
            this.groupBoxSelected.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxVideoId;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Label labelVideoId;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Label labelVideoTitel;
        private System.Windows.Forms.Label labelVideoTitelValue;
        private System.Windows.Forms.Label labelAudioSizeValue;
        private System.Windows.Forms.Label labelAudioSize;
        private System.Windows.Forms.Label labelVideoSizeValue;
        private System.Windows.Forms.Label labelVideoSize;
        private System.Windows.Forms.Label labelTotalSizeValue;
        private System.Windows.Forms.Label labelTotalSize;
        private System.Windows.Forms.ToolTip toolTipTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonAddDownload;
        private System.Windows.Forms.GroupBox groupBoxAllVideos;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBoxSelected;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

