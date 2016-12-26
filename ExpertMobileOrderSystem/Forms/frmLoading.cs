using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace ExpertMobileOrderSystem
{
    public partial class frmLoading : Form
    {
        //private static Thread _splashThread;
        //private static ExpertMobileSystem.frmLoading _splashForm;
        //private delegate void CloseDelegate();
        public frmLoading()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Show the Splash Screen (Loading...)
        /// </summary>
        //public static void ShowSplash()
        //{
        //    if (_splashThread == null)
        //    {
        //        // show the form in a new thread
        //        _splashThread = new Thread(new ThreadStart(DoShowSplash));
        //        _splashThread.IsBackground = true;
        //        _splashThread.Start();
        //    }
        //}

        //// called by the thread
        //private static void DoShowSplash()
        //{
        //    //if (_splashForm == null)
        //    //{
        //        _splashForm = new frmLoading();
        //        //_splashForm.MdiParent=;
        //    //}

        //    // create a new message pump on this thread (started from ShowSplash)
        //    Application.Run(_splashForm);
        //}

        ///// <summary>
        ///// Close the splash (Loading...) screen
        ///// </summary>
        //public static void CloseSplash()
        //{
        //    // need to call on the thread that launched this splash
        //    if (_splashForm.InvokeRequired)
        //    {
        //        _splashForm.Invoke(new MethodInvoker(CloseSplash));
        //    }
        //    else
        //    {
        //        Application.ExitThread();
        //        _splashForm = null;
        //        _splashThread = null;
        //    }
        //    //_splashForm.Invoke(new CloseDelegate(frmLoading.CloseFormInternal));
        //}
        //private static void CloseFormInternal()
        //{
        //    _splashForm.Close();
        //    _splashForm = null;
        //}
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;
        }
        }
}
