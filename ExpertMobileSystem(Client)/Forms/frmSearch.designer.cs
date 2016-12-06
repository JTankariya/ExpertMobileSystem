namespace ExpertMobileSystem_Client_
{
    partial class frmSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkisglobal = new System.Windows.Forms.CheckBox();
            this.txtsearch = new System.Windows.Forms.TextBox();
            this.cmbsearchon = new System.Windows.Forms.ComboBox();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // chkisglobal
            // 
            this.chkisglobal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkisglobal.AutoSize = true;
            this.chkisglobal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkisglobal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkisglobal.Location = new System.Drawing.Point(510, 10);
            this.chkisglobal.Name = "chkisglobal";
            this.chkisglobal.Size = new System.Drawing.Size(80, 17);
            this.chkisglobal.TabIndex = 6;
            this.chkisglobal.Text = "Is Global?";
            this.chkisglobal.UseVisualStyleBackColor = true;
            // 
            // txtsearch
            // 
            this.txtsearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsearch.Location = new System.Drawing.Point(168, 8);
            this.txtsearch.Name = "txtsearch";
            this.txtsearch.Size = new System.Drawing.Size(322, 21);
            this.txtsearch.TabIndex = 5;
            this.txtsearch.TextChanged += new System.EventHandler(this.txtsearch_TextChanged);
            // 
            // cmbsearchon
            // 
            this.cmbsearchon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbsearchon.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbsearchon.FormattingEnabled = true;
            this.cmbsearchon.Location = new System.Drawing.Point(7, 7);
            this.cmbsearchon.Name = "cmbsearchon";
            this.cmbsearchon.Size = new System.Drawing.Size(140, 21);
            this.cmbsearchon.TabIndex = 8;
            this.cmbsearchon.SelectionChangeCommitted += new System.EventHandler(this.cmbsearchon_SelectionChangeCommitted);
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToDeleteRows = false;
            this.dgvSearch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.dgvSearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Location = new System.Drawing.Point(6, 35);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.dgvSearch.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSearch.Size = new System.Drawing.Size(608, 360);
            this.dgvSearch.TabIndex = 7;
            this.dgvSearch.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearch_CellDoubleClick);
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(619, 400);
            this.Controls.Add(this.chkisglobal);
            this.Controls.Add(this.txtsearch);
            this.Controls.Add(this.cmbsearchon);
            this.Controls.Add(this.dgvSearch);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Records";
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmSearch_KeyPress);
            this.Resize += new System.EventHandler(this.frmSearch_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkisglobal;
        internal System.Windows.Forms.TextBox txtsearch;
        internal System.Windows.Forms.ComboBox cmbsearchon;
        public  System.Windows.Forms.DataGridView dgvSearch;
    }
}