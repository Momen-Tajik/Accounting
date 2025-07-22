using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App
{
    public partial class FrmAddOrEditCustomer : Form
    {
        public int customerId = 0;
        UnitOfWork db=new UnitOfWork();
        public FrmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK) 
            {
                picCustomer.ImageLocation=openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string imageName=Guid.NewGuid().ToString()+Path.GetExtension(picCustomer.ImageLocation);
                string path=Application.StartupPath+"/Images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                picCustomer.Image.Save(path+imageName);
                Customers customers = new Customers()
                {
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    EmailAddress = txtEmail.Text,
                    Address = txtAddress.Text,
                    CustomerImage = imageName
                };

                customers.CustomerID = customerId;
                if (customerId == 0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                }
                else
                {
                    db.CustomerRepository.UpdateCustomer(customers);
                }
                db.Save();
                DialogResult = DialogResult.OK;
            }
        }

        private void FrmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {
                this.Text = "ویرایش افراد";
                btnSave.Text = "ویرایش";
                var customer=db.CustomerRepository.GetCustomerById(customerId);
                txtAddress.Text = customer.Address;
                txtName.Text=customer.FullName;
                txtMobile.Text=customer.Mobile;
                txtEmail.Text=customer.EmailAddress;
                picCustomer.ImageLocation=Application.StartupPath+"/Images"+customer.CustomerImage;
                
            }
        }
    }
}
