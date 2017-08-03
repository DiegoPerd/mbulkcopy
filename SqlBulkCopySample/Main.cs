using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using DllSqlBulkCopy;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;

namespace SqlBulkCopySample
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
            SourceBox.SelectedItem = SourceBox.Items[0];
            DestBox.SelectedItem = DestBox.Items[0];
        }

        public clsCopy clsC;

        delegate void CopyDelegate(bool bAppendData, out String ret);

        private void btnStart_Click(object sender, EventArgs e)
        {

      

            int iThreads;
            iThreads = int.Parse(txtThreads.Text);
            clsC = new clsCopy(txtSourceTable.Text, txtDestinationTable.Text, txtSource.Text, txtDestination.Text, iThreads);

            clsC.SourceTableScheme = txtSourceScheme.Text;            

            clsC.CheckConstraints = chkCheckConstraints.Checked;
            clsC.FireTriggers = chkFireTriggers.Checked;
            clsC.KeepIdentity = chkKeepIdentity.Checked;
            clsC.KeepNulls = chkKeepNulls.Checked;
            clsC.TabLock = chkTablock.Checked;
            clsC.NoLock = chkNoLock.Checked;
            clsC.HeapMethod = chkHeap.Checked;
            if (DestBox.SelectedItem.ToString()=="SQL Server")
                clsC.DestinationBBDDType = BBDDType.SqlServer;
            else
                clsC.DestinationBBDDType = BBDDType.Oracle;

            if (SourceBox.SelectedItem.ToString() == "SQL Server")
                clsC.SourceBBDDType= BBDDType.SqlServer;
            else
                clsC.SourceBBDDType = BBDDType.Oracle;

            clsC.SelectFilter = txtSelectFilter.Text;

            clsC.NotifyAfter = int.Parse(txtNotifyAfter.Text);
            clsC.SqlRowsCopied += new clsCopy.SqlRowsCopiedDelegate(OnSqlRowsCopied);
            clsC.ThreadFinished += new clsCopy.SqlThreadFinishedDelegate(OnThreadEnd);



            listBox1.Items.Clear();
            for (int i = 0; i < iThreads; i++)
            {
                listBox1.Items.Add("Thread " + i);
            }

            this.Enabled = false;

            CopyDelegate dlgt = new CopyDelegate(this.StartCopy);
            AsyncCallback cb = new AsyncCallback(CopyFinished);
            String ret;
            IAsyncResult ar = dlgt.BeginInvoke(chkAppend.Checked, out ret, cb, dlgt);
        }

        public void StartCopy(bool bAppendData, out String sret)
        {
            String ret = clsC.Copy(bAppendData);
            sret = ret;
        }

        public void CopyFinished(IAsyncResult ar)
        {
            String ret;

            CopyDelegate dlgt = (CopyDelegate)ar.AsyncState;

            dlgt.EndInvoke(out ret, ar);

            if (ret != "")
                MessageBox.Show(ret);

            SetFormEnabled();
        }

        delegate void SetFormEnabledCallback();

        private void SetFormEnabled()
        {
            if (this.InvokeRequired)
            {
                SetFormEnabledCallback d = new SetFormEnabledCallback(SetFormEnabled);
                this.Invoke(d);
            }
            else
            {
                this.Enabled = true;
            }
        }

        public void OnSqlRowsCopied(
        object sender, long lRowsCopied, IclsThreadCopy oTc)
        {
            SetThreadText(oTc, lRowsCopied, "");
        }

        public void OnThreadEnd(object sender, IclsThreadCopy oTc, String sErr)
        {
            SetThreadText(oTc, oTc.Size, sErr);
        }

        delegate void SetThreadTextCallback(IclsThreadCopy oTc, long lRowsCopied, String sErrg);

        private void SetThreadText(IclsThreadCopy oTc, long lRowsCopied, String sErr)
        {
            if (listBox1.InvokeRequired)
            {
                SetThreadTextCallback d = new SetThreadTextCallback(SetThreadText);
                this.Invoke(d, new object[] { oTc, lRowsCopied, sErr });
            }
            else
            {
                if (sErr == "")
                {
                    if (String.IsNullOrEmpty(clsC.SelectFilter))
                    {
                        if (lRowsCopied < oTc.Size)
                        {
                            listBox1.Items[oTc.IdThread] = "Thread " + oTc.IdThread + ": " + lRowsCopied + " / " + oTc.Size + " rows copied.";
                        }
                        else
                        {
                            if(lRowsCopied>0)
                                listBox1.Items[oTc.IdThread] = "Thread " + oTc.IdThread + " finished. " + lRowsCopied + " rows copied.";
                        }
                    }
                    else
                    {
                        if (lRowsCopied < oTc.Size)
                        {
                            listBox1.Items[oTc.IdThread] = "Thread " + oTc.IdThread + ": " + lRowsCopied + " rows copied.";
                        }
                        else
                        {
                            listBox1.Items[oTc.IdThread] = "Thread " + oTc.IdThread + " finished.";
                        }
                    }
                }
                else
                {
                    listBox1.Items[oTc.IdThread] = "Error in Thread " + oTc.IdThread + ": " + sErr;
                }
            }
        }

        #region Connection Tests

        private void btoSourceTest_Click(object sender, EventArgs e)
        {
            try
            {
                TestConnection(SourceBox.SelectedItem.ToString(),txtSource.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btoDestinationTest_Click(object sender, EventArgs e)
        {
            TestConnection(DestBox.SelectedItem.ToString(), txtDestination.Text);

        }

        private void TestConnection(string BBDDType, string ConnectionString)
        {
            switch (BBDDType)
            {

                case "SQL Server":
                    {
                        using (SqlConnection sql = new SqlConnection(ConnectionString))
                        {
                            sql.Open();
                        }
                        MessageBox.Show("Connection performed successfully.");
                        break;
                    }
                case "Oracle":
                    {
                        using (OracleConnection sql = new OracleConnection(ConnectionString))
                        {
                            sql.Open();
                        }
                        MessageBox.Show("Connection performed successfully.");
                        break;
                    }
            }
        }

      

        #endregion




    }
}
