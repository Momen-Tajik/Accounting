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
    public partial class FrmLogin : Form
    {
        public bool IsEdit=false;
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                this.Text = "تنظیمات ورود به برنامه";
                btnLogin.Text = "ذخیره";
                using(UnitOfWork db=new UnitOfWork())
                {
                    var login=db.LoginRepository.GetAll().First();
                    txtUserName.Text = login.UserName;
                    txtPassword.Text = login.Password;
                }

            }            
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork()) 
                {
                    if (IsEdit)
                    {
                        var login=db.LoginRepository.GetAll().First();
                        login.UserName = txtUserName.Text;
                        login.Password = txtPassword.Text;
                        db.LoginRepository.Update(login);
                        db.Save();
                        Application.Restart();
                    }
                    else
                    {
                        if (db.LoginRepository.GetAll(l => l.UserName == txtUserName.Text && l.Password == txtPassword.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            RtlMessageBox.Show("رمز عبور یا نام کاربری اشتباه است");
                        }
                    }
                }
            }

        }
    }
}
