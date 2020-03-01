using Customer.Controllers;
using Customer.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestCustomer
{
    [TestClass]
    public class CustomerUnitTest
    {
        readonly string _connectionString = "Server=.\\SQLExpress;Database=CustomerDb;Trusted_Connection=True;";
        
        #region Unit Tests for CustomerDb
        [TestMethod]
        public void Db_NonExistentRecord()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);

            // act
            var customer = customerDb.Customers.SingleOrDefault(c => c.Id == 0);

            // assert
            Assert.IsTrue(customer == null);
        }

        [TestMethod]
        public void Db_GetCustomerById()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            customer = customerDb.AddCustomer(customer);
            customer = customerDb.Customers.SingleOrDefault(c => c.Id == customer.Id);

            // assert
            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Db_GetCustomers()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);

            // act
            var customers = customerDb.Customers;

            // assert
            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void Db_CreateCustomer()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            customer = customerDb.AddCustomer(customer);

            // assert
            Assert.IsTrue(customer.Id > 0);
        }

        [TestMethod]
        public void Db_UpdateCustomer()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act
            customer = customerDb.AddCustomer(customer);
            customer.FirstName = "Marlo";            
            customer = customerDb.UpdateCustomer(customer);

            // assert
            Assert.AreEqual(customer.FirstName, "Marlo");
        }

        [TestMethod]
        public void Db_UpdateNonExistentCustomer()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act
            customer.FirstName = "Marlo";
            customer = customerDb.UpdateCustomer(customer);

            // assert
            Assert.IsTrue(customer == null);
        }

        [TestMethod]
        public void Db_DeleteCustomer()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "xtt99rtw" };

            // act
            customer = customerDb.AddCustomer(customer);
            var result = customerDb.DeleteCustomer(customer);

            // assert
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void Db_DeleteNoExistentCustomer()
        {
            // arrange
            var customerDb = new CustomerDb(_connectionString);
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "xtt99rtw" };

            // act
            var result = customerDb.DeleteCustomer(customer);

            // assert
            Assert.AreEqual(result, 0);
        }

        #endregion

        #region Unit Tests for CustomerController using the actual database
        [TestMethod]
        public void Api_NonExistentCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));

            // act
            var result = customerController.Get(0);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void Api_GetCustomerById()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };


            // act
            var result1= customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;
            var result2 = customerController.Get(customer.Id);
            customer = (Customer.EF.Customer)((OkObjectResult)result2).Value;

            // assert
            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Api_GetCustomers()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));

            // act
            var result = customerController.All();
            var customers = (IList<Customer.EF.Customer>)((OkObjectResult)result).Value;

            // assert
            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void Api_CreateCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result).Value;

            // assert
            Assert.IsTrue(customer.Id > 0);
        }

        [TestMethod]
        public void Api_UpdateCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act
            var result1 = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;

            customer.FirstName = "Marlo";
            var result2 = customerController.Update(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result2).Value;

            // assert
            Assert.AreEqual(customer.FirstName,"Marlo");
            Assert.AreEqual(customer.LastName, "Smith");
        }

        [TestMethod]
        public void Api_UpdateNonExistentCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act            

            customer.FirstName = "Marlo";
            var result = customerController.Update(customer);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void Api_DeleteCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result1 = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;
            var result2 = customerController.Delete(customer.Id);
            var result3 = (bool)((OkObjectResult)result2).Value;

            // assert
            Assert.IsTrue(result3);
        }


        [TestMethod]
        public void Api_DeleteNonExistentCustomer()
        {
            // arrange
            CustomerController customerController = new CustomerController(new CustomerDb(_connectionString));
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result = customerController.Delete(customer.Id);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        #endregion

        #region Unit Test for CustomerController using an in-memory databasse
        [TestMethod]
        public void Api_NonExistentCustomer_InMemoryDb()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());

            // act
            var result = customerController.Get(0);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void Api_GetCustomerById_InMemoryDb()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };


            // act
            var result1 = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;
            var result2 = customerController.Get(customer.Id);
            customer = (Customer.EF.Customer)((OkObjectResult)result2).Value;

            // assert
            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Api_GetCustomers_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());

            // act
            var result = customerController.All();
            var customers = (IList<Customer.EF.Customer>)((OkObjectResult)result).Value;

            // assert
            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void Api_CreateCustomer_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result).Value;

            // assert
            Assert.IsTrue(customer.Id > 0);
        }

        [TestMethod]
        public void Api_UpdateCustomer_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act
            var result1 = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;

            customer.FirstName = "Marlo";
            var result2 = customerController.Update(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result2).Value;

            // assert
            Assert.AreEqual(customer.FirstName, "Marlo");
            Assert.AreEqual(customer.LastName, "Smith");
        }

        [TestMethod]
        public void Api_UpdateNonExistentCustomer_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "Marli", LastName = "Smith", Password = "xtt99rtw" };

            // act            

            customer.FirstName = "Marlo";
            var result = customerController.Update(customer);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void Api_DeleteCustomer_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result1 = customerController.Create(customer);
            customer = (Customer.EF.Customer)((OkObjectResult)result1).Value;
            var result2 = customerController.Delete(customer.Id);
            var result3 = (bool)((OkObjectResult)result2).Value;

            // assert
            Assert.IsTrue(result3);
        }

        [TestMethod]
        public void Api_DeleteNonExistentCustomer_InMemory()
        {
            // arrange
            CustomerController customerController = new CustomerController(new InMemoryCustomerDb());
            var customer = new Customer.EF.Customer { Id = 0, FirstName = "John", LastName = "Doddle", Password = "x099rtw" };

            // act
            var result = customerController.Delete(customer.Id);
            var notFoundResult = (NotFoundResult)result;

            // assert
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        #endregion

    }
}
