using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class FrmReport : Form
    {
        public int typeId=0;
        public FrmReport()
        {
            InitializeComponent();
        }

        private void FrmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID=0,
                    FullName="انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetCustomersName(""));
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
                //List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                //list.Add(new ListCustomerViewModel()
                //{
                //    CustomerID = 0,
                //    FullName = "انتخاب کنید"
                //});

                //list.AddRange(db.CustomerRepository.GetCustomersName());
            }
            if (typeId == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
            Filter();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }
        void Filter()
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;

                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId=int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.GetAll(a=>a.CustomerID == customerId && a.TypeID == typeId));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.GetAll(a=>a.TypeID == typeId));
                }
                
                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    //startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTitle >= startDate).ToList();
                    
                    
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    //endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTitle <= endDate.Value).ToList();
                }

                dgvReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string customerName=db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgvReport.Rows.Add(accounting.AccountingID, customerName,accounting.Amount,accounting.DateTitle.ToShamsi(),accounting.Description);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDeleteReport_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null) 
            {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());
                if (RtlMessageBox.Show("آیا از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (UnitOfWork db=new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
            else 
            {
                RtlMessageBox.Show("لطفا یک گزارش را انتخاب کنید!");
            }
        }

        private void btnEditReport_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());
                FrmNewTransaction frmNewTransaction = new FrmNewTransaction();
                frmNewTransaction.AccountID = id;   
                if (frmNewTransaction.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
            else
            {
                RtlMessageBox.Show("لطفا یک گزارش را انتخاب کنید!");
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow row in dtPrint.Rows) {
                dtPrint.Rows.Add(
                    row.Cells[0].Value.ToString(),
                    row.Cells[1].Value.ToString(),
                    row.Cells[2].Value.ToString(),
                    row.Cells[3].Value.ToString()
                );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT",dtPrint);
            stiPrint.Dictionary.Synchronize();
            stiPrint.Compile();
            stiPrint.Render();
            //stiPrint.RegData("txtTime",DateConvertor.ToShamsi(DateTime.Now));
            stiPrint.Show();
        }
    }
}
