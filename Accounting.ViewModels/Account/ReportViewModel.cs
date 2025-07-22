using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.ViewModels.Account
{
    public class ReportViewModel
    {
        public decimal Resive {  get; set; }
        public decimal Pay {  get; set; }
        public decimal AccountBalance {  get; set; }
        public string DebtorOrCreditor {  get; set; }
    }
}
