namespace ExpertMobileOrderSystem
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnSync = new System.Windows.Forms.Button();
            this.toolUploadStatus = new System.Windows.Forms.StatusStrip();
            this.toolUploadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolUploadStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSync
            // 
            this.btnSync.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.Image = global::ExpertMobileOrderSystem.Properties.Resources._1482787339_sync;
            this.btnSync.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSync.Location = new System.Drawing.Point(12, 40);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(183, 80);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Start Sync";
            this.btnSync.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // toolUploadStatus
            // 
            this.toolUploadStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolUploadProgress,
            this.toolStripStatusLabel1});
            this.toolUploadStatus.Location = new System.Drawing.Point(0, 384);
            this.toolUploadStatus.Name = "toolUploadStatus";
            this.toolUploadStatus.Size = new System.Drawing.Size(665, 22);
            this.toolUploadStatus.TabIndex = 1;
            // 
            // toolUploadProgress
            // 
            this.toolUploadProgress.Name = "toolUploadProgress";
            this.toolUploadProgress.Size = new System.Drawing.Size(100, 16);
            this.toolUploadProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Name :";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(665, 406);
            this.Controls.Add(this.toolUploadStatus);
            this.Controls.Add(this.btnSync);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Sync Utility";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolUploadStatus.ResumeLayout(false);
            this.toolUploadStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.StatusStrip toolUploadStatus;
        private System.Windows.Forms.ToolStripProgressBar toolUploadProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

