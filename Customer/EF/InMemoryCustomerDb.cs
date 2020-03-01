using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.EF
{
    public class InMemoryCustomerDb : ICustomerDb
    {
        public IList<Customer> Customers { get; } = new List<Customer>();

        public Customer AddCustomer(Customer customer)
        {
            var id = Customers.Count + 1;
            customer.Id = id;
            Customers.Add(customer);
            return customer;
        }

        public int DeleteCustomer(Customer customer)
        {
            var deletedCustomer = Customers.SingleOrDefault(c => c.Id == customer.Id);

            if (deletedCustomer == null) return 0;

            Customers.Remove(deletedCustomer);
            return 1;
        }

        public Customer GetCustomerById(int customerId)
        {
            return Customers.SingleOrDefault(c => c.Id == customerId);
        }

        public IList<Customer> GetCustomersByFirstName(string firstName)
        {
            return Customers.Where(c => c.FirstName == firstName).ToList();
        }

        public IList<Customer> GetCustomersByLastName(string lastName)
        {
            return Customers.Where(c => c.LastName == lastName).ToList();
        }

        public Customer UpdateCustomer(Customer customer)
        {
            var updatedCustomer = Customers.SingleOrDefault(c => c.Id == customer.Id);

            if (updatedCustomer == null)
            {
                return null;                
            }

            Customers.Remove(updatedCustomer);
            Customers.Add(customer);
            return customer;
        }
    }
}
