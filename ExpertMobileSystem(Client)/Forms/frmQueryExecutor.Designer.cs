namespace ExpertMobileSystem_Client_
{
    partial class frmQueryExecutor
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
            this.dgvresult = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvresult)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvresult
            // 
            this.dgvresult.AllowDrop = true;
            this.dgvresult.AllowUserToAddRows = false;
            this.dgvresult.AllowUserToDeleteRows = false;
            this.dgvresult.AllowUserToResizeRows = false;
            this.dgvresult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvresult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvresult.BackgroundColor = System.Drawing.Color.White;
            this.dgvresult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvresult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvresult.Location = new System.Drawing.Point(3, 3);
            this.dgvresult.Name = "dgvresult";
            this.dgvresult.ReadOnly = true;
            this.dgvresult.RowHeadersVisible = false;
            this.dgvresult.Size = new System.Drawing.Size(662, 396);
            this.dgvresult.TabIndex = 0;
            // 
            // frmQueryExecutor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(668, 402);
            this.Controls.Add(this.dgvresult);
            this.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmQueryExecutor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Executor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvresult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvresult;

    }
}