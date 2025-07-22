using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App
{
    public partial class FrmNewTransaction : Form
    {
        public int AccountID = 0;
        private UnitOfWork db;
        public FrmNewTransaction()
        {
            InitializeComponent();
        }

        private void FrmNewTransaction_Load(object sender, EventArgs e)
        {
            db= new UnitOfWork();
            dgvCustomers.AutoGenerateColumns=false;
            dgvCustomers.DataSource=db.CustomerRepository.GetCustomersName();
            if (AccountID != 0)
            {
                var account=db.AccountingRepository.GetById(AccountID);
                txtAmount.Text = account.Amount.ToString();
                txtDescription.Text = account.Description.ToString();
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                if (account.TypeID == 1)
                {
                    rbResive.Checked = true;
                }
                else 
                {
                    rbPay.Checked = true;
                }
                this.Text = "ویرایش";
                btnSave.Text = "ثبت";
                db.Dispose();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.DataSource=db.CustomerRepository.GetCustomersName(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if(rbPay.Checked || rbResive.Checked)
                {
                    db = new UnitOfWork();
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        Amount = decimal.Parse(txtAmount.Text),
                        TypeID = (rbResive.Checked) ? 1 : 2,
                        DateTitle=DateTime.Now,
                        Description=txtDescription.Text
                    };
                    if(AccountID==0)
                    {
                        db.AccountingRepository.Add(accounting);
                    }
                    else
                    {
                        accounting.AccountingID = AccountID;
                        db.AccountingRepository.Update(accounting);
                    }
                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    RtlMessageBox.Show("لطفا نوع تراکنش را مشخص کنید!");
                }
            }
        }
    }
}
