using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.EF
{
    public class CustomerDb : ICustomerDb
    {
        readonly CustomerDbContext _dbContext;

        public CustomerDb(string connectionString)
        {
            _dbContext = new CustomerDbContext(connectionString);
        }
        public CustomerDb(IConfiguration configuration)
        {
            _dbContext = new CustomerDbContext(configuration.GetConnectionString("CustomersConnectionString"));
        }
        public IList<Customer> Customers { get { return _dbContext.Customer.ToList(); } }

        public Customer AddCustomer(Customer customer)
        {
            _dbContext.Customer.Add(customer);
            _dbContext.SaveChanges();
            return customer;
        }

        public int DeleteCustomer(Customer customer)
        {
            var deletedCustomer = _dbContext.Customer.SingleOrDefault(c => c.Id == customer.Id);

            if (deletedCustomer == null) return 0;

            _dbContext.Customer.Remove(customer);
            return _dbContext.SaveChanges();
        }

        public Customer UpdateCustomer(Customer customer)
        {
            var updatingCustomer = _dbContext.Customer.SingleOrDefault(c => c.Id == customer.Id);

            if (updatingCustomer != null)
            {
                updatingCustomer.FirstName = customer.FirstName;
                updatingCustomer.LastName = customer.LastName;
                updatingCustomer.Password = customer.Password;

                _dbContext.Update(updatingCustomer);
                _dbContext.SaveChanges();

            }
            return updatingCustomer;
        }
        
        public Customer GetCustomerById(int customerId)
        {
            return _dbContext.Customer.SingleOrDefault(c => c.Id == customerId);
        }

        public IList<Customer> GetCustomersByFirstName(string firstName)
        {
            return _dbContext.Customer.Where(c => c.FirstName == firstName).ToList();
        }
        
        public IList<Customer> GetCustomersByLastName(string lastName)
        {
            return _dbContext.Customer.Where(c => c.LastName == lastName).ToList();
        }
        
    }
}
