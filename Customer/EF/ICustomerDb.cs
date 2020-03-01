using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.EF
{
    public interface ICustomerDb
    {
        IList<Customer> Customers { get;}

        Customer AddCustomer(Customer customer);

        Customer UpdateCustomer(Customer customer);

        int DeleteCustomer(Customer customer);

        Customer GetCustomerById(int customerId);

        IList<Customer> GetCustomersByFirstName(string firstName);

        IList<Customer> GetCustomersByLastName(string lastName);
    }
}
