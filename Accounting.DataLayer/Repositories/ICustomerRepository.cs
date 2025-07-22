using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository
    {
        List<Customers> GetCustomersName();
        IEnumerable<Customers> GetCustomersByFilter(string parametr);
        List<ListCustomerViewModel> GetCustomersName(string filter = "");
        Customers GetCustomerById(int customerId);
        int GetCustomerIdByName(string name);
        string GetCustomerNameById(int customerId);
        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerId);
    }
}
