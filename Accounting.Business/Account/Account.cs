using Accounting.DataLayer.Context;
using Accounting.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Business.Account
{
    public class Account
    {
        public static ReportViewModel ReportMainForm() 
        {
            ReportViewModel rp =new ReportViewModel();
            using (UnitOfWork db = new UnitOfWork()) 
            {
                DateTime startDate=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(-31);
                DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                var resive=db.AccountingRepository.GetAll(a=>a.TypeID==1 && a.DateTitle>=startDate && a.DateTitle<=endDate).Select(a=>a.Amount).ToList();
                var pay=db.AccountingRepository.GetAll(a=>a.TypeID==2 && a.DateTitle>=startDate && a.DateTitle<=endDate).Select(a=>a.Amount).ToList();
                rp.Resive = (decimal)resive.Sum();
                rp.Pay = (decimal)pay.Sum();
                rp.AccountBalance = (decimal)(resive.Sum() - pay.Sum());
                if (rp.AccountBalance > 0)
                {
                    rp.DebtorOrCreditor = "بدهکار";
                }
                else if(rp.AccountBalance < 0)
                {
                    rp.DebtorOrCreditor = "طلبکار";
                    rp.AccountBalance = rp.AccountBalance*-1;
                }
                else
                {
                    rp.DebtorOrCreditor = "";
                }
            }
            return rp;
        }
    }
}
