using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace ExpertMobileSystem_Client_
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            Paint += draw;
            Invalidate();
            try
            { this.Icon = new System.Drawing.Icon(Application.StartupPath + "\\MOBILE.ico"); }
            catch { }
        }
        private void draw(object sender, PaintEventArgs e)
        {
            try
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, Operation.strFirstColor, Operation.strSecondColor, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            catch { }
        }
        private void frmSettings_Load(object sender, EventArgs e)
        {

            Operation.GetIniValue();
            cmbDataUploadedOption.Text = Operation.DataUploadedOption;
            DateTime startTime = DateTime.ParseExact("12:00 AM", "hh:mm tt", new DateTimeFormatInfo());
            DateTime endTime = new DateTime();
            string StringData = Operation.TimeInterval;
            string[] TimeArray = StringData.Split(',');
//                DateTime.ParseExact("12:00 PM", "HH:mm tt", new DateTimeFormatInfo());
            for (int i = 0; i <= 23; i++)
            {
                endTime = startTime.Date + new TimeSpan(i, 0, 0);
                lstTimeInterval.Items.Add(endTime.ToString("hh:mm tt"));
                int pos = Array.IndexOf(TimeArray, endTime.ToString("hh:mm tt"));
                //TimeArray.All(s=>endTime.ToShortTimeString().Trim().Equals(s))
                if(pos>-1)
                {
                    lstTimeInterval.SetItemChecked(i,true);
                }
            }
            chkPrompt.Checked = Operation.PromptBeforeData;
            txtMin.Text = Operation.PromptMins.ToString();
            chkAfterUpload.Checked = Operation.PromptAfterData;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Operation.DataUploadedOption = cmbDataUploadedOption.Text;
                Operation.TimeInterval = "";
                for (int i = 0; i < lstTimeInterval.Items.Count; i++)
                {
                    if (lstTimeInterval.GetItemCheckState(i) == CheckState.Checked)
                    {
                        Operation.TimeInterval = Operation.TimeInterval + lstTimeInterval.Items[i].ToString() + ",";
                    }
                }
                if (Operation.TimeInterval == "")
                {
                    Operation.TimeInterval = "01:00 PM";
                }
                else
                {
                    Operation.TimeInterval = Operation.TimeInterval.Remove(Operation.TimeInterval.Length - 1, 1);
                }
                Operation.PromptBeforeData = chkPrompt.Checked;
                Operation.PromptMins = Convert.ToInt32(string.IsNullOrEmpty(txtMin.Text) ? "0" : txtMin.Text);
                Operation.PromptAfterData = chkAfterUpload.Checked;
                Operation.SetIniValue();
                MessageBox.Show("Save Successfully...." + Environment.NewLine , Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while Saving, Please Try after Some Time." + Environment.NewLine + ex.Message, Operation.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void chkPrompt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrompt.Checked == true)
            {
                txtMin.Enabled = true;
            }
            else
            {
                txtMin.Text = "";
                txtMin.Enabled = false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

   
    }
}
