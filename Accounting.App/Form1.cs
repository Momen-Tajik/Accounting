using Accounting.Business.Account;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmMain : Form
    {
        public void updateTime() 
        {
            lblDate.Text = DateConvertor.ToShamsi(DateTime.Now).ToString();
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Hide();
            FrmLogin frmLogin = new FrmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                updateTime();
                mainFormReport();
            }
            else
            {
                Application.Exit();
            }
        }

        void mainFormReport()
        {
            ReportViewModel report = Account.ReportMainForm();
            lblResive.Text=report.Resive.ToString("#,0");
            lblPay.Text=report.Pay.ToString("#,0");
            lblAccountBalance.Text=report.AccountBalance.ToString("#,0");
            lblDebtorOrCreditor.Text = report.DebtorOrCreditor.ToString();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            FrmCustomers frmCustomers = new FrmCustomers();
            frmCustomers.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmNewTransaction frmNewTransaction = new FrmNewTransaction();
            frmNewTransaction.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            FrmReport frmReport = new FrmReport();
            frmReport.typeId = 2;
            frmReport.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmReport frmReport = new FrmReport();
            frmReport.typeId = 1;
            frmReport.ShowDialog();
        }

        private void updateTime(object sender, EventArgs e)
        {
            updateTime();
            mainFormReport();
        }

        private void تنظیماتورودToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.IsEdit = true;
            frmLogin.ShowDialog();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            mainFormReport();
            updateTime();
        }

        private void lblDebtorOrCreditor_Click(object sender, EventArgs e)
        {

        }
    }
}
